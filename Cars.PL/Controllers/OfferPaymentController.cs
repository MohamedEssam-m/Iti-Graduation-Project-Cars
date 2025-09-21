using AutoMapper;
using Cars.BLL.Helper;
using Cars.BLL.ModelVM.Offers;
using Cars.BLL.ModelVM.Payment;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.DAL.Entities;
using Cars.DAL.Entities.Accidents;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace Cars.PL.Controllers
{
    public class OfferPaymentController : Controller
    {
        private readonly IPayPalService paypalService;
        private readonly IOfferService offerService;
        private readonly IAccidentService accidentService;
        private readonly UserManager<AppUser> userManager;
        private readonly PayPal Paypal;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public OfferPaymentController(IEmailService emailService,IMapper mapper,IPayPalService paypalService,IOfferService offerService,IAccidentService accidentService,UserManager<AppUser> userManager,IConfiguration configuration)
        {
            this.paypalService = paypalService;
            this.offerService = offerService;
            this.accidentService = accidentService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.emailService = emailService;

            Paypal = new PayPal
            {
                PayPalClientId = configuration["PayPal:ClientId"],
                PayPalSecret = configuration["PayPal:ClientSecret"],
                PayPalUrl = configuration["PayPal:URL"]
            };
        }

        
        public async Task<IActionResult> PaymentOfferSummary(int offerId)
        {
            var offer = await offerService.GetOfferById(offerId);
            if (offer == null)
            {
                ViewBag.Error = "Offer not found!";
                return RedirectToAction("Repair", "Accident");
            }

            var accident = await accidentService.GetAccidentById(offer.AccidentId);
            var userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);

            if (accident == null || user == null)
            {
                ViewBag.Error = "Something went wrong!";
                return RedirectToAction("Repair", "Accident");
            }

            var summary = new OfferPaymentSummaryVM
            {
                OfferId = offer.OfferId,
                AccidentId = accident.AccidentId,
                MechanicName = offer.Mechanic?.FullName,
                MechanicEmail = offer.Mechanic?.Email,
                CarName = offer.CarName,
                Price = offer.Price,
                StartRepair = offer.OfferStartDate,
                ReceiveCar = offer.OfferEndDate,
                UserName = user.FullName,
                UserEmail = user.Email
            };

            ViewBag.PayPalClientId = Paypal.PayPalClientId;

            return View(summary);
        }

        [HttpGet]
        public async Task<IActionResult> GetPayPalAccessToken()
        {
            var token = await paypalService.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
                return BadRequest("Failed to get PayPal Access Token.");

            return Ok(new { AccessToken = token });
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["total"]?.ToString();
            if (string.IsNullOrEmpty(totalAmount))
                return Json(new { error = "Total amount is required." });

            var createOrderRequest = new JsonObject
            {
                ["intent"] = "CAPTURE",
                ["purchase_units"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["amount"] = new JsonObject
                        {
                            ["currency_code"] = "USD",
                            ["value"] = totalAmount
                        }
                    }
                }
            };

            var accessToken = await paypalService.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
                return Json(new { error = "Failed to get PayPal Access Token" });

            var url = Paypal.PayPalUrl + "/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(createOrderRequest.ToString(), null, "application/json")
                };

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new { Id = paypalOrderId });
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderID"]?.ToString();
            var offerId = data?["offerId"]?.ToString();

            if (orderId == null || offerId == null)
                return new JsonResult("error");

            string accessToken = await paypalService.GetAccessTokenAsync();
            string url = Paypal.PayPalUrl + "/v2/checkout/orders/" + orderId + "/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent("", null, "application/json")
                };

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string status = jsonResponse["status"]?.ToString() ?? "";
                        if (status.ToUpper() == "COMPLETED")
                        {
                            var offer = await offerService.GetOfferById(int.Parse(offerId));
                            if (offer != null)
                            {
                                offer.Status = "Paid";
                                var offerVM = mapper.Map<UpdateOfferVM>(offer);
                                bool updateResult = await offerService.UpdateOffer(offerVM);
                                if (updateResult)
                                {
                                    var AccidentId = offer.AccidentId;
                                    var MechanicName = offer.Mechanic?.UserName;
                                    var MechanicEmail = offer.Mechanic?.Email;
                                    //can not use "Url" in the service , due to this reason I used SendAcceptedOfferEmail to be in the middle
                                    var link = Url.Action("Offers", "Accident", new { accidentId = AccidentId }, protocol: HttpContext.Request.Scheme);
                                    await emailService.SendAcceptedOfferEmail(MechanicEmail, MechanicName, AccidentId, link);
                                    return new JsonResult("success");
                                }
                            }
                            
                        }
                    }
                }
                return new JsonResult("error");
            }
        }
    }
}

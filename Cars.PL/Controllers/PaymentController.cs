using Cars.BLL.Helper;
using Cars.BLL.Helper.Renting;
using Cars.BLL.ModelVM.CarVM;
using Cars.BLL.ModelVM.Payment;
using Cars.BLL.ModelVM.Rent;
using Cars.BLL.ModelVM.RentVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities;
using Cars.DAL.Entities.Renting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;
using AutoMapper;

namespace Cars.PL.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPayPalService paypalService;
        private readonly ICarService carService;
        private readonly UserManager<AppUser> userManager;
        private readonly PayPal Paypal;
        private readonly IRentService rentService;
        private readonly IMapper mapper;

        public PaymentController(IMapper mapper, IPayPalService paypalService,ICarService carService,UserManager<AppUser> userManager,IConfiguration configuration , IRentService rentService)
        {
            this.paypalService = paypalService;
            this.carService = carService;
            this.userManager = userManager;
            this.rentService = rentService;
            this.mapper = mapper;

            Paypal = new PayPal
            {
                PayPalClientId = configuration["PayPal:ClientId"],
                PayPalSecret = configuration["PayPal:ClientSecret"],
                PayPalUrl = configuration["PayPal:URL"]
            };
        }

        [HttpPost]
        public async Task<IActionResult> PaymentSummary(CreateRentVM model)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Something went wrong!";
                return View("RentingDetails", model);
            }
            else
            {
                var car = await carService.GetById(model.CarId);
                var userId = userManager.GetUserId(User);
                var user = await userManager.FindByIdAsync(userId);
                model.UserId = userId;

                if (car == null || user == null)
                {
                    ViewBag.Error = "Something went wrong!";
                    return View("RentingDetails", model);
                }

                var duration = (model.EndDate - model.StartDate).Days;
                if (duration <= 0)
                {
                    ViewBag.Error = "Invalid Duration!";
                    return View("RentingDetails", model);
                }

                var summary = new PaymentSummaryVM
                {
                    UserId = userId,
                    CarId = car.CarId,
                    CarName = car.Brand,
                    CarImage = car.CarImagePath,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Duration = duration,
                    PricePerDay = car.PricePerDay,
                    TotalAmount = duration * car.PricePerDay,
                    UserName = user.FullName,
                    Pick_up_location = model.Pick_up_location,
                    Drop_Off_location = model.Drop_Off_location
                };
                //var rent = new Rent
                //{
                //    CarId = model.CarId,
                //    UserId = userId,
                //    StartDate = model.StartDate,
                //    EndDate = model.EndDate,
                //    Pick_up_location = model.Pick_up_location,
                //    Drop_Off_location = model.Drop_Off_location,
                //    Payment = new RentPayment
                //    {
                //        Amount = summary.TotalAmount,
                //        IsDone = false,
                //        PaymentDate = DateTime.UtcNow
                //    }
                //};
                await rentService.CreateRent(summary);

                ViewBag.PayPalClientId = Paypal.PayPalClientId;


                return View(summary);
            }
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
            if (orderId == null) return new JsonResult("error");

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
                            var userId = userManager.GetUserId(User);
                            var user = await userManager.FindByIdAsync(userId);
                            if (user != null && user.Rents != null)
                            {
                                foreach (var item in user.Rents)
                                {
                                    if (item.Payment != null && !item.Payment.IsDone)
                                    {
                                        item.Payment.IsDone = true;
                                        item.Status = "Paid";
                                        await rentService.UpdateRent(new UpdateRentVM
                                        {
                                            RentId = item.RentId,
                                            StartDate = item.StartDate,
                                            EndDate = item.EndDate,
                                            Pick_up_location = item.Pick_up_location,
                                            Drop_Off_location = item.Drop_Off_location,
                                            Status = item.Status,
                                            IsDone = item.Payment.IsDone,
                                            TotalAmount = item.Payment.Amount
                                        });
                                        var rent = await rentService.GetRentById(item.RentId);
                                        var car = await carService.GetById(rent.CarId);
                                        if (car != null && car.quantity != 0)
                                        {
                                            var carVM = mapper.Map<UpdateCarVM>(car);
                                            carVM.quantity--;
                                            if (carVM.quantity == 0)
                                            {
                                                carVM.Status = "Not Available";
                                            }
                                            await carService.Update(carVM);
                                        }
                                    }
                                }
                            }
                            //Create a User Page to add new rents and
                            //show his rents and their status
                            
                            return new JsonResult("success");
                        }
                    }
                }
                return new JsonResult("error");
            }
        }
    }
}

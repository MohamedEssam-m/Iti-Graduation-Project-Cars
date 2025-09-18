using AutoMapper;
using Cars.BLL.ModelVM.Accident;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.ModelVM.Offers;
using Cars.BLL.ModelVM.Payment;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.DAL.Entities.Accidents;
using Cars.DAL.Entities.Offers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService offerService;
        private readonly IMapper mapper;
        private readonly IAccidentService accidentService;
        private readonly IAppUserService appUserService;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly ICarService carService;
        private readonly IPayPalService payPalService;

        public OfferController(ICarService carService, IEmailService emailService, UserManager<AppUser> userManager, IMapper mapper, IOfferService offerService, IAccidentService accidentService, IAppUserService appUserService, IPayPalService payPalService)
        {
            this.offerService = offerService;
            this.mapper = mapper;
            this.accidentService = accidentService;
            this.appUserService = appUserService;
            this.userManager = userManager;
            this.emailService = emailService;
            this.carService = carService;
            this.payPalService = payPalService;
        }


        public async Task<IActionResult> Index(int accidentId)
        {
            var offers = await offerService.GetOffersByAccident(accidentId);
            ViewBag.AccidentId = accidentId;
            return View(offers);
        }


        [HttpGet]
        public async Task<IActionResult> CreateView(int accidentId)
        {
            var Accident = await accidentService.GetAccidentById(accidentId);
            var car = await carService.GetById(Accident.carId);
            var carName = car.Brand;
            var Mechanic = await userManager.GetUserAsync(User);
            var MechanicId = Mechanic.Id;
            var offer = new Offer { AccidentId = accidentId };
            var offerVM = mapper.Map<CreateOfferVM>(offer);
            offerVM.AccidentId = accidentId;
            offerVM.CarName = carName;
            //offerVM.MechanicId = MechanicId;
            return View(offerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferVM offer)
        {
            if (ModelState.IsValid)
            {
                var IsTrue = await offerService.AddOffer(offer);
                if (IsTrue)
                {
                    var Mechanic = await userManager.GetUserAsync(User);
                    var Accident = await accidentService.GetAccidentById(offer.AccidentId);
                    var user = await appUserService.GetById(Accident.UserId);
                    ViewBag.Success = "The Offer Sent Successfully";
                    return RedirectToAction("SendOfferEmail", "Offer", new { userEmail = user.Email, mechanicName = Mechanic.FullName, accidentId = Accident.AccidentId });
                    //return View("Create" , offer);
                }

            }
            return View("CreateView", offer);
        }
        public async Task<ActionResult> SendOfferEmail(string userEmail, string mechanicName, int accidentId)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                ViewBag.Error = "User not found!";
                return View("CreateView", new CreateOfferVM());
            }

            var offersLink = Url.Action("Offers", "Accident", new { accidentId = accidentId }, protocol: HttpContext.Request.Scheme);

            var subject = "New Offer Received!";
            var body = $@"
        <div style='font-family: Arial, sans-serif; color: #333;'>
            <h2>You've got a new offer 🚗🔧</h2>
            <p>Hello {user.UserName},</p>
            <p>You have received a new offer from <strong>{mechanicName}</strong> for your accident report.</p>
            <p>Click the button below to review the offer:</p>
            <a href='{offersLink}' 
               style='display:inline-block; padding:10px 20px; margin-top:10px;
                      background-color:#28a745; color:#fff; text-decoration:none;
                      border-radius:5px;'>
               View Offers
            </a>
            <p style='margin-top:20px; font-size:12px; color:#777;'>If you did not request this, please ignore this email.</p>
        </div>
    ";

            await emailService.SendEmail(userEmail, subject, body);

            ViewBag.Success = "Offer Email Sent";
            return View("CreateView", new CreateOfferVM());
        }


        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var accepted = await offerService.AcceptOffer(id);
            if (accepted)
            {


                var offer = await offerService.GetOfferById(id);
                //Redirect to payment

                return RedirectToAction("PaymentOfferSummary", "OfferPayment", new { offerId = id });
                //var accident = await accidentService.GetAccidentById(offer.AccidentId);
                //var user = await appUserService.GetById(accident.UserId);
                //var car = await carService.GetById(accident.carId);
                //ViewBag.Success = "The Offer Accepted Successfully";
                //return RedirectToAction("SendAcceptedOfferEmail", "Offer", new { userEmail = user.Email, mechanicName = offer.Mechanic?.FullName, accidentId = accident.AccidentId });
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View();
        }
        public async Task<ActionResult> SendAcceptedOfferEmail(string MechanicEmail, string mechanicName, int accidentId)
        {


            ViewBag.Success = "Offer Email Sent";
            return View("CreateView", new CreateOfferVM());
        }


        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var accepted = await offerService.DeclineOffer(id);
            if (accepted)
            {
                var offer = await offerService.GetOfferById(id);
                ViewBag.Success = "Offerd Declined Successfully";
                return View("Offers", "Accident");
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("Offers", "Accident");
        }
        //public async Task<IActionResult> PaymentOfferSummary(OfferPaymentSummaryVM offerVM)
        //{
        //    var offer = await offerService.GetOfferById(offerVM.OfferId);
        //    var Accident = await accidentService.GetAccidentById(offerVM.AccidentId);
        //    var User = await appUserService.GetById(Accident.UserId);
        //    offerVM.StartRepair = offer.OfferStartDate;
        //    offerVM.ReceiveCar = offer.OfferEndDate;
        //    offerVM.Price = offer.Price;
        //    offerVM.UserName = User.UserName;
        //    offerVM.UserEmail = User.Email;
        //    offerVM.MechanicName = offer.Mechanic?.UserName;
        //    offerVM.MechanicEmail = offer.Mechanic?.Email;
        //    offerVM.CarName = offer.CarName;

        //    return View(offerVM);
        //}
        [HttpGet]
        public async Task<IActionResult> EditOfferView(int offerId)
        {
            var offer = await offerService.GetOfferById(offerId);
            if(offer == null || offer.OfferId == 0)
            {
                ViewBag.Error = "Some Thing Was Wrong";
                var updateOfferVM = mapper.Map<UpdateOfferVM>(offer);
                return View("Repair", updateOfferVM);
            }
            
                //ViewBag.Error = "Something went wrong!";
                //var listOfOffers = await offerService.GetOffersByAccident(offer.AccidentId);
                //var updateOfferVM = new UpdateOfferVM { OfferId = offerId};
                //return View(updateOfferVM);
            

            var offerVM = mapper.Map<UpdateOfferVM>(offer);
            offerVM.OfferId = offerId;
            return View(offerVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditOffer(UpdateOfferVM offer)
        {
            if (ModelState.IsValid && offer.OfferId > 0)
            {
                var isUpdated = await offerService.UpdateOffer(offer);
                if (isUpdated)
                {
                    ViewBag.Success = "The Offer Updated Successfully";
                    return View("EditOfferView", offer);
                }
            }
            ViewBag.Error = "Something went wrong!";
            return View("EditOfferView", offer);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOfferView(int offerId)
        {
            var offer = await offerService.GetOfferById(offerId);

            if (offer == null || offer.OfferId == 0)
            {
                ViewBag.Error = "Some Thing Was Wrong";
                var updateOfferVM = mapper.Map<UpdateOfferVM>(offer);
                return View("Repair" , updateOfferVM);
            }
            return View(offer);
            //ViewBag.Error = "Something went wrong!"





        }
        [HttpPost]
        public async Task<IActionResult> Delete(int offerId)
        {
            var isDeleted = await offerService.DeleteOffer(offerId);
            if (isDeleted)
            {
                ViewBag.Success = "The Offer Deleted Successfully";
                return View("DeleteOfferView"); 
            }
            var offer = await offerService.GetOfferById(offerId);
            var updateOfferVM = mapper.Map<UpdateOfferVM>(offer);
            ViewBag.Error = "Something went wrong!";
            return View("Repair", updateOfferVM);
            
            


        }
    }
}
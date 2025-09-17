using AutoMapper;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.ModelVM.Offers;
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
       

        public OfferController(ICarService carService,IEmailService emailService,UserManager<AppUser> userManager,IMapper mapper,IOfferService offerService, IAccidentService accidentService, IAppUserService appUserService)
        {
            this.offerService = offerService;
            this.mapper = mapper;
            this.accidentService = accidentService;
            this.appUserService = appUserService;
            this.userManager = userManager;
            this.emailService = emailService;
            this.carService = carService;
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
                    return RedirectToAction("SendOfferEmail", "Offer", new { userEmail = user.Email , mechanicName =  Mechanic.FullName , accidentId  = Accident.AccidentId});
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
            return View("CreateView" , new CreateOfferVM());
        }


        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var accepted = await offerService.AcceptOffer(id);
            if(accepted)
            {
                var offer = await offerService.GetOfferById(id);
                return RedirectToAction("Index", new { accidentId = offer.AccidentId });
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Decline(int id)
        {
            var accepted = await offerService.DeclineOffer(id);
            if (accepted)
            {
                var offer = await offerService.GetOfferById(id);
                return RedirectToAction("Index", new { accidentId = offer.AccidentId });
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View();
        }
    }
}

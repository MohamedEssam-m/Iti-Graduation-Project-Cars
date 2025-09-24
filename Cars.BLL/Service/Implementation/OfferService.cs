namespace Cars.BLL.Service.Implementation
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepo offerRepo;
        private readonly IAccidentRepo accidentRepo;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;
        private readonly IAccidentService accidentService;
        private readonly ICarService carService;
        private readonly IConfiguration configuration;

        public OfferService(IConfiguration configuration,ICarService carService,IAccidentService accidentService,IMapper mapper,IOfferRepo offerRepo, IAccidentRepo accidentRepo, IEmailService emailService)
        {
            this.offerRepo = offerRepo;
            this.accidentRepo = accidentRepo;
            this.emailService = emailService;
            this.mapper = mapper;
            this.accidentService = accidentService;
            this.carService = carService;
            this.configuration = configuration;
        }

        public async Task<bool> AddOffer(CreateOfferVM offer)
        {
            if (offer == null) 
            { 
                throw new ArgumentNullException(); 
            }
            var mainOffer = mapper.Map<Offer>(offer);
            mainOffer.Status = "Pending"; 
            await offerRepo.Add(mainOffer);
            return true;
        }

        public async Task<Offer> GetOfferById(int id)
        {
            var offer = await offerRepo.GetById(id);
            if (offer == null)
            {
                throw new ArgumentNullException();
            }
            return offer;
        }

        public async Task<List<Offer>> GetOffersByAccident(int accidentId)
        {
            var accident = await offerRepo.GetByAccidentId(accidentId);
            if (accident == null)
            {
                throw new ArgumentNullException();
            }
            return accident;
        }

        public async Task<bool> UpdateOffer(UpdateOfferVM offer)
        {
            if(offer != null)
            {
                var mainOffer = mapper.Map<Offer>(offer);
                bool IsTrue = await offerRepo.Update(mainOffer);
                if (IsTrue)
                {
                    return IsTrue;
                }
            }
            throw new InvalidOperationException();

        }

        public async Task<bool> DeleteOffer(int id)
        {
            var offer = await offerRepo.GetById(id);
            if (offer == null)
            {
                throw new ArgumentNullException();
            }
            await offerRepo.Delete(id);
            return true;
        }

        public async Task<bool> AcceptOffer(int offerId)
        {
            
            var offer = await offerRepo.GetById(offerId);
            if (offer != null)
            {
                offer.Status = "Pending";
                await offerRepo.Update(offer);

                return true;

                
            }
            throw new ArgumentNullException();
        }

        public async Task<bool> DeclineOffer(int offerId)
        {
            var offer = await offerRepo.GetById(offerId);
            if (offer != null)
            {
                offer.Status = "Declined";
                await offerRepo.Update(offer);
                return true;
            }
            throw new ArgumentNullException();
        }
        public async Task CheckOfferDate()
        {
            var AllAccidents = await accidentService.GetAllAccidents();
            foreach (var accident in AllAccidents)
            {
                if(accident.Offers!=null && accident.Offers.Count > 0)
                {
                    foreach (var offer in accident.Offers)
                    {
                        if(offer.Status == "Paid" && offer.OfferEndDate < DateTime.UtcNow)
                        {
                            var car = await carService.GetById(accident.carId);
                            TimeSpan diff = DateTime.UtcNow - offer.OfferEndDate;
                            offer.Fine = Math.Abs(car.PricePerDay * 2 * ((diff).Minutes));
                            var updateOffer = mapper.Map<UpdateOfferVM>(offer);
                            updateOffer.Fine = offer.Fine;
                            bool result = await UpdateOffer(updateOffer);
                            if (result)
                            {
                                string subject = $"🚨 Notification of Fine for Late Car Return - Offer #{offer.OfferId}";

                                string body = $@"
<div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%); padding: 20px; border-radius: 16px; box-shadow: 0 8px 32px rgba(0,0,0,0.1);'>
    <!-- Header Section -->
    <div style='background: linear-gradient(135deg, #dc2626, #b91c1c); color: white; padding: 25px; border-radius: 12px 12px 0 0; text-align: center; margin: -20px -20px 20px -20px;'>
        <div style='font-size: 48px; margin-bottom: 10px;'>
            🔧💸
        </div>
        <h1 style='margin: 0; font-size: 28px; font-weight: 700; text-shadow: 0 2px 4px rgba(0,0,0,0.2);'>
            REPAIR OVERDUE NOTICE
        </h1>
        <p style='margin: 8px 0 0 0; font-size: 16px; opacity: 0.9;'>
            Fine Applied to Your Account
        </p>
    </div>

    <!-- Main Content -->
    <div style='background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 16px rgba(0,0,0,0.05); margin-bottom: 20px;'>
        <!-- Greeting -->
        <div style='margin-bottom: 25px;'>
            <h2 style='color: #1e293b; font-size: 24px; font-weight: 600; margin: 0 0 10px 0;'>
                Dear {offer.Mechanic?.FullName},
            </h2>
            <p style='color: #64748b; font-size: 16px; line-height: 1.6; margin: 0;'>
                We hope this message finds you well.
            </p>
        </div>

        <!-- Alert Box -->
        <div style='background: linear-gradient(135deg, #fef2f2, #fee2e2); border: 2px solid #fecaca; border-left: 6px solid #dc2626; padding: 20px; border-radius: 10px; margin: 25px 0;'>
            <div style='display: flex; align-items: flex-start; gap: 15px;'>
                <div style='font-size: 24px; margin-top: 2px;'>⚠️</div>
                <div>
                    <h3 style='color: #991b1b; font-size: 18px; font-weight: 700; margin: 0 0 8px 0;'>
                        OVERDUE REPAIR NOTIFICATION
                    </h3>
                    <p style='color: #b91c1c; font-size: 15px; line-height: 1.5; margin: 0;'>
                        This is to inform you that the car repair offer <strong>#{offer.OfferId}</strong> associated with your account has exceeded the scheduled end date. As a result, a fine has been applied to your account.
                    </p>
                </div>
            </div>
        </div>

        <!-- Fine Details Section -->
        <div style='background: linear-gradient(135deg, #fef3c7, #fde68a); border: 2px solid #fbbf24; padding: 25px; border-radius: 12px; margin: 25px 0; box-shadow: 0 4px 12px rgba(251, 191, 36, 0.2);'>
            <h3 style='color: #92400e; font-size: 20px; font-weight: 700; margin: 0 0 15px 0; display: flex; align-items: center; gap: 10px;'>
                💰 Fine Details
            </h3>
            <div style='background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                <div style='display: grid; gap: 12px;'>
                    <div style='display: flex; justify-content: space-between; align-items: center; padding: 10px 0; border-bottom: 1px solid #e2e8f0;'>
                        <span style='color: #64748b; font-weight: 500;'>Vehicle:</span>
                        <span style='color: #1e293b; font-weight: 700; font-size: 16px;'>{car.Brand} {car.Model}</span>
                    </div>
                    <div style='display: flex; justify-content: space-between; align-items: center; padding: 10px 0; border-bottom: 1px solid #e2e8f0;'>
                        <span style='color: #64748b; font-weight: 500;'>Original End Date:</span>
                        <span style='color: #dc2626; font-weight: 700; font-size: 16px; background: #fef2f2; padding: 4px 12px; border-radius: 20px;'>{offer.OfferEndDate:MMM dd, yyyy}</span>
                    </div>
                    <div style='display: flex; justify-content: space-between; align-items: center; padding: 10px 0;'>
                        <span style='color: #64748b; font-weight: 500;'>Fine Amount:</span>
                        <span style='color: #dc2626; font-weight: 900; font-size: 20px; background: linear-gradient(135deg, #fef2f2, #fee2e2); padding: 8px 16px; border-radius: 25px; border: 2px solid #fecaca; text-shadow: 0 1px 2px rgba(0,0,0,0.1);'>${offer.Fine:F2}</span>
                    </div>
                </div>
            </div>
        </div>

        <!-- Important Notice Section -->
        <div style='background: linear-gradient(135deg, #eff6ff, #dbeafe); border: 1px solid #93c5fd; border-left: 4px solid #3b82f6; padding: 20px; border-radius: 10px; margin: 20px 0;'>
            <h4 style='color: #1e40af; font-size: 16px; font-weight: 600; margin: 0 0 10px 0; display: flex; align-items: center; gap: 8px;'>
                ⏰ Future Reference
            </h4>
            <p style='color: #1e40af; font-size: 14px; line-height: 1.5; margin: 0; font-weight: 500;'>
                Please ensure <strong>timely completion of car repairs</strong> to avoid further fines.
            </p>
        </div>

        

        <!-- Closing Message -->
        <div style='text-align: center; margin: 25px 0 10px 0; padding: 20px; background: #f8fafc; border-radius: 8px;'>
            <p style='color: #475569; font-size: 16px; line-height: 1.6; margin: 0 0 10px 0;'>
                Thank you for your attention and cooperation.
            </p>
            <p style='color: #64748b; font-size: 15px; font-weight: 600; margin: 0;'>
                Best regards,<br>
                <span style='color: #1e293b; font-weight: 700;'>[Your Company Name]</span>
            </p>
        </div>
    </div>

    <!-- Mobile Responsive Meta -->
    <!--[if mso | IE]>
    <style>
        .email-container 
    <![endif]-->
</div>";

                                await emailService.SendEmail(offer.Mechanic?.Email, subject, body, configuration["EmailSettings:From"]);
                            }
                            
                        }
                    }
                }
            }
        }
    }

}

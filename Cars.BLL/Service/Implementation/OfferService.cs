using AutoMapper;
using Cars.BLL.ModelVM.Offers;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Offers;
using Cars.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Implementation
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepo offerRepo;
        private readonly IAccidentRepo accidentRepo;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;

        public OfferService(IMapper mapper,IOfferRepo offerRepo, IAccidentRepo accidentRepo, IEmailService emailService)
        {
            this.offerRepo = offerRepo;
            this.accidentRepo = accidentRepo;
            this.emailService = emailService;
            this.mapper = mapper;
        }

        public async Task<bool> AddOffer(CreateOfferVM offer)
        {
            if (offer == null) 
            { 
                throw new ArgumentNullException(); 
            }
            var mainOffer = mapper.Map<Offer>(offer);
            mainOffer.Status = "Pending"; // Default status
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
                offer.Status = "Accepted";
                await offerRepo.Update(offer);

                // Send email to mechanic
                var mechanicEmail = offer.Mechanic?.Email;
                if (!string.IsNullOrEmpty(mechanicEmail))
                {
                    await emailService.SendEmail(
                        mechanicEmail,
                        "Offer Accepted",
                        $"Your offer for accident #{offer.AccidentId} has been accepted"
                    );
                    return true;
                }

                // redirect to payment
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
    }

}

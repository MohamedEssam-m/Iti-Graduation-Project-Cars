namespace Cars.BLL.Service.Abstraction
{
    public interface IOfferService
    {
        Task<bool> AddOffer(CreateOfferVM offer);
        Task<Offer> GetOfferById(int offerId);
        Task<List<Offer>> GetOffersByAccident(int accidentId);
        Task<bool> UpdateOffer(UpdateOfferVM offer);
        Task<bool> DeleteOffer(int offerId);
        Task<bool> AcceptOffer(int offerId);
        Task<bool> DeclineOffer(int offerId);
        public Task CheckOfferDate();
    }
}

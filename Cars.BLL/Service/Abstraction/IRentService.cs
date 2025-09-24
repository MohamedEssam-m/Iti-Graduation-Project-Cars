namespace Cars.BLL.Service.Abstraction
{
    public interface IRentService
    {
        Task<bool> CreateRent(PaymentSummaryVM rent);
        Task<bool> DeleteRent(int id);
        Task<Rent?> GetRentById(int id);
        Task<List<Rent>> GetAllRents();
        Task<bool> UpdateRent(UpdateRentVM rent);
        public Task CheckRentDate();
    }
}

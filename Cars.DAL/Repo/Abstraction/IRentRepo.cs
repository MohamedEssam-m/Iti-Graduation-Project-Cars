namespace Cars.DAL.Repo.Abstraction
{
    public interface IRentRepo
    {
        Task<bool> CreateRent(Rent rent);
        Task<bool> DeleteRent(Rent rent);
        Task<Rent?> GetRentById(int id);
        Task<List<Rent>> GetAllRents();
        Task<bool> UpdateRent(Rent rent);
    }
}

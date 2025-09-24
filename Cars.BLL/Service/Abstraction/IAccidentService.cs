namespace Cars.BLL.Service.Abstraction
{
    public interface IAccidentService
    {
        Task<bool> AddAccident(CreateAccidentVM accident , string UserId);
        Task<Accident> GetAccidentById(int id);
        Task<List<Accident>> GetAllAccidents();
        Task<bool> UpdateAccident(UpdateAccidentVM accident);
        Task<bool> DeleteAccident(int id);
    }
}

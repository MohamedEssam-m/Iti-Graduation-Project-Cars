namespace Cars.DAL.Repo.Abstraction
{
    public interface IAccidentRepo
    {
        Task<bool> Add(Accident accident);
        Task<Accident> GetById(int id);
        Task<List<Accident>> GetAll();
        Task<bool> Update(Accident accident);
        Task<bool> Delete(int id);
    }
}

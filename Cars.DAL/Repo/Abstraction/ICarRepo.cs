namespace Cars.DAL.Repo.Abstraction
{
    public interface ICarRepo
    {
        public void Add(Car user);
        public List<Car> GetAll();
        public void Update(Car user);
        public void Delete(int id);
        public Car GetById(int id);
    }
}

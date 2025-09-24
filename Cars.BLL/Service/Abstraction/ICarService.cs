namespace Cars.BLL.Service.Abstraction
{
    public interface ICarService
    {
        public Task<bool> Add(CreateCarVM user);
        public Task<List<Car>> GetAll();
        public Task<List<Car>> GetAllRandom();
        public Task<bool> Update(UpdateCarVM user);
        public Task<bool> Delete(int id);
        public Task<Car> GetById(int id);
        public Task<bool> RateCar(RateCarVM rateCarVM , string UserId);
    }
}

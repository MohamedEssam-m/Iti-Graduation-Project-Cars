namespace Cars.BLL.Service.Abstraction
{
    public interface IMechanicService
    {
        public Task Add(CreateMechanicVM user);
        public List<MechanicUser> GetAll();
        public void Update(UpdateMechanicVM user);
        public void Delete(string id);
        public MechanicUser GetById(string id);
    }
}

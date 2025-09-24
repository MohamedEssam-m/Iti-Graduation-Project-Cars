namespace Cars.DAL.Repo.Abstraction
{
    public interface IMechanicRepo
    {
        void CreateMechanic(MechanicUser mechanic);
        MechanicUser GetMechanicById(string mechanicId);
        List<MechanicUser> GetAllMechanics();
        void UpdateMechanic(MechanicUser mechanic);
        void DeleteMechanic(string mechanicId);
    }
}

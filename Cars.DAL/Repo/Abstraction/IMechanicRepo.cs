using Cars.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using Cars.DAL.Entities.Renting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

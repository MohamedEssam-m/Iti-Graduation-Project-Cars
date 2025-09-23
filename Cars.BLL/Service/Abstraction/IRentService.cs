using Cars.BLL.ModelVM.Payment;
using Cars.BLL.ModelVM.Rent;
using Cars.BLL.ModelVM.RentVM;
using Cars.DAL.Entities.Renting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

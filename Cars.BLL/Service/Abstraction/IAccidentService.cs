using Cars.BLL.ModelVM.Accident;
using Cars.DAL.Entities.Accidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

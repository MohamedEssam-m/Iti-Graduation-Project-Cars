using Cars.DAL.Entities.Accidents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

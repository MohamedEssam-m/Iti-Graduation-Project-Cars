using Cars.DAL.Entities.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Abstraction
{
    public interface IOfferRepo
    {
        Task<bool> Add(Offer offer);
        Task<Offer> GetById(int id);
        Task<List<Offer>> GetByAccidentId(int accidentId);
        Task<bool> Update(Offer offer);
        Task<bool> Delete(int id);
    }
}

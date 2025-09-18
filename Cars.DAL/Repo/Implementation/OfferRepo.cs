using Cars.DAL.Database;
using Cars.DAL.Entities.Offers;
using Cars.DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Implementation
{
    public class OfferRepo : IOfferRepo
    {
        private readonly CarsDbContext db;

        public OfferRepo(CarsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Add(Offer offer)
        {
            try
            {
                if (offer == null) throw new ArgumentNullException(nameof(offer));

                db.Offers.Add(offer);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Offer> GetById(int id)
        {
            try
            {
                return db.Offers
                         .Include(o => o.Mechanic)
                         .Include(o => o.Accident)
                         .FirstOrDefault(o => o.OfferId == id) ?? new Offer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Offer();
            }
        }

        public async Task<List<Offer>> GetByAccidentId(int accidentId)
        {
            try
            {
                return db.Offers
                         .Include(o => o.Mechanic)
                         .Where(o => o.AccidentId == accidentId)
                         .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Offer>();
            }
        }

        public async Task<bool> Update(Offer offer)
        {
            try
            {
                var oldOffer = db.Offers.FirstOrDefault(o => o.OfferId == offer.OfferId);
                if (oldOffer != null)
                {
                    oldOffer.Price = offer.Price;
                    oldOffer.Details = offer.Details;
                    oldOffer.OfferStartDate = offer.OfferStartDate;
                    oldOffer.OfferEndDate = offer.OfferEndDate;
                    db.Offers.Update(oldOffer);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var offer = db.Offers.FirstOrDefault(o => o.OfferId == id);
                if (offer != null)
                {
                    db.Offers.Remove(offer);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}

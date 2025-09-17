using Cars.DAL.Database;
using Cars.DAL.Entities.Accidents;
using Cars.DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Implementation
{
    public class AccidentRepo : IAccidentRepo
    {
        private readonly CarsDbContext db;

        public AccidentRepo(CarsDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> Add(Accident accident)
        {
            try
            {
                if (accident == null) throw new ArgumentNullException(nameof(accident));

                db.Accidents.Add(accident);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<Accident> GetById(int id)
        {
            try
            {
                return db.Accidents
                         .Include(a => a.User)
                         .Include(a => a.Offers)
                         .ThenInclude(o => o.Mechanic)
                         .FirstOrDefault(a => a.AccidentId == id) ?? new Accident();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Accident();
            }
        }

        public async Task<List<Accident>> GetAll()
        {
            try
            {
                return db.Accidents
                         .Include(a => a.User)
                         .Include(a => a.Offers)
                         .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Accident>();
            }
        }

        public async Task<bool> Update(Accident accident)
        {
            try
            {
                var oldAccident = db.Accidents.FirstOrDefault(a => a.AccidentId == accident.AccidentId);
                if (oldAccident != null)
                {
                    oldAccident.Description = accident.Description;
                    oldAccident.Location = accident.Location;
                    oldAccident.ReportDate = accident.ReportDate;
                    db.Accidents.Update(oldAccident);
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
                var accident = db.Accidents.FirstOrDefault(a => a.AccidentId == id);
                if (accident != null)
                {
                    db.Accidents.Remove(accident);
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

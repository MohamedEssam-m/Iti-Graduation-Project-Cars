using Cars.DAL.Database;
using Cars.DAL.Entities.Users;
using Cars.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Implementation
{
    public class MechanicRepo : IMechanicRepo
    {
        private readonly CarsDbContext db;

        public MechanicRepo(CarsDbContext db)
        {
            this.db = db;
        }

        // Create
        public void CreateMechanic(MechanicUser mechanic)
        {
            try
            {
                if (mechanic == null) throw new ArgumentNullException(nameof(mechanic));

                db.MechanicUsers.Add(mechanic);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Read mechanic by Id
        public MechanicUser GetMechanicById(string id)
        {
            try
            {
                if (id != null)
                {
                    var result = db.MechanicUsers.FirstOrDefault(m => m.Id == id);
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
                return new MechanicUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new MechanicUser();
            }
        }

        // Read all mechanics
        public List<MechanicUser> GetAllMechanics()
        {
            try
            {
                return db.MechanicUsers.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<MechanicUser>();
            }
        }

        // Update
        public void UpdateMechanic(MechanicUser mechanic)
        {
            try
            {
                if (mechanic == null) throw new ArgumentNullException(nameof(mechanic));

                var existingMechanic = db.MechanicUsers.FirstOrDefault(m => m.Id == mechanic.Id);
                if (existingMechanic != null)
                {
                    db.Entry(existingMechanic).CurrentValues.SetValues(mechanic);
                    db.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Delete
        public void DeleteMechanic(string id)
        {
            try
            {
                var mechanic = db.MechanicUsers.FirstOrDefault(m => m.Id == id);
                if (mechanic != null)
                {
                    db.MechanicUsers.Remove(mechanic);
                    db.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

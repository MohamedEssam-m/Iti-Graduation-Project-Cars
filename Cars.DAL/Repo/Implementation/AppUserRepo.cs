using Cars.DAL.Database;
using Cars.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Implementation
{
    
    public class AppUserRepo : IAppUserRepo
    {
        private readonly CarsDbContext db;
        public AppUserRepo(CarsDbContext db)
        {
            this.db= db;
        }
        public void Add(AppUser user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));

                db.Users.Add(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Read single user by Id
        public AppUser GetById(string id)
        {
            try
            {
                if(id != null)
                {
                    var result =  db.Users.FirstOrDefault(u => u.Id == id);
                    if(result != null)
                    { 
                        return result; 
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
                return new AppUser();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AppUser();
            }
        }

        // Read all users
        public List<AppUser> GetAll()
        {
            try
            {
                return db.Users.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<AppUser>();
            }
        }

        // Update
        public void Update(AppUser user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user));

                var existingUser = db.Users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    db.AppUsers.Update(existingUser);
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
        public void Delete(string id)
        {
            try
            {
                var user = db.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
                throw new ArgumentNullException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}

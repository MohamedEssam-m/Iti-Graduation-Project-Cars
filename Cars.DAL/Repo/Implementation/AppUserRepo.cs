using Cars.DAL.Database;
using Cars.DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace Cars.DAL.Repo.Implementation
{
    
    public class AppUserRepo : IAppUserRepo
    {
        private readonly CarsDbContext db;
        private readonly IMapper mapper;
        public AppUserRepo(CarsDbContext db , IMapper mapper)
        {
            this.db= db;
            this.mapper= mapper;
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
                    var result =  db.Users.Include(user => user.Rents).ThenInclude(r => r.Car).FirstOrDefault(u => u.Id == id);
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
                return db.Users.Include(user => user.Rents).ThenInclude(user => user.Car).ToList();
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
                if (user == null) 
                    throw new Exception();

                var existingUser = db.AppUsers.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    existingUser.FullName = user.FullName;
                    existingUser.UserName = user.UserName;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Address = user.Address;
                    existingUser.Age = user.Age;
                    db.AppUsers.Update(existingUser);
                    db.SaveChanges(); 
                }
                else
                {
                    throw new Exception("User not found");
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

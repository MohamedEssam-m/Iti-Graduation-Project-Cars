using Cars.DAL.Database;
using Cars.DAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Implementation
{
    public class CarRepo : ICarRepo
    {
        private readonly CarsDbContext db;
        public CarRepo(CarsDbContext db)
        {
            this.db = db;
        }
        public void Add(Car car)
        {
            try
            {
                if (car == null) throw new ArgumentNullException(nameof(car));

                db.Cars.Add(car);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Read single user by Id
        public Car GetById(int id)
        {
            try
            {
                var result = db.Cars.Include(c => c.CarRates).FirstOrDefault(c => c.CarId == id);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Car();
            }
        }

        // Read all users
        public List<Car> GetAll()
        {
            try
            {
                return db.Cars.Include(c => c.CarRates).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Car>();
            }
        }

        // Update
        public void Update(Car car)
        {
            try
            {
                if (car == null) throw new ArgumentNullException(nameof(car));

                var existingcar =  db.Cars
                .Include(c => c.CarRates)
                .FirstOrDefault(c => c.CarId == car.CarId);
                if (existingcar != null && existingcar.CarRates != null)
                {
                    existingcar.AverageRating = car.AverageRating;
                    foreach (var rate in car.CarRates)
                    {
                        if (existingcar.CarRates.Any(r => r.UserId == rate.UserId && r.CarId == rate.CarId))
                        {
                            var rateToUpdate = existingcar.CarRates.First(r => r.UserId == rate.UserId && r.CarId == rate.CarId);
                            rateToUpdate.Rating = rate.Rating;
                            rateToUpdate.Comment = rate.Comment;
                            rateToUpdate.UserFullName = rate.UserFullName;

                        }
                        else
                            existingcar.CarRates.Add(rate);

                    }
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
        public void Delete(int id)
        {
            try
            {
                var car = db.Cars.FirstOrDefault(u => u.CarId == id);
                if (car != null)
                {
                    db.Cars.Remove(car);
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

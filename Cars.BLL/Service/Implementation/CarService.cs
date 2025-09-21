using AutoMapper;
using Cars.BLL.Helper;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.CarVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Cars;
using Cars.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Implementation
{
    public class CarService : ICarService
    {
        private readonly ICarRepo repo;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        public CarService(ICarRepo repo, IMapper mapper , UserManager<AppUser> userManager)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        // Add Car
        public async Task<bool> Add(CreateCarVM CarVM)
        {
            try
            {
                var imagePath = Upload.UploadFile("Files", CarVM.Car_Image);
                var mappedCar = new Car
                {
                    CarImagePath = imagePath,
                    Brand = CarVM.Brand,
                    Model = CarVM.Model,
                    Year = CarVM.Year,
                    BodyType = CarVM.BodyType,
                    Doors = CarVM.Doors,
                    FuelType = CarVM.FuelType,
                    EngineCapacity = CarVM.EngineCapacity,
                    HorsePower = CarVM.HorsePower,
                    FuelConsumption = CarVM.FuelConsumption,
                    Seats = CarVM.Seats
                };
                if (CarVM == null)
                    return false;
                else
                {
                    var car = mapper.Map<Car>(CarVM);
                    repo.Add(car);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // Delete Car
        public async Task<bool> Delete(int carId)
        {
            try
            {
                var car = repo.GetById(carId);
                if (car != null && car.CarId > 0)
                {
                    repo.Delete(carId);
                    return true;
                }
                else
                {
                    throw new Exception("Car not found");
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // Get All Cars
        public async Task<List<Car>> GetAll()
        {
            try
            {
                var list = repo.GetAll();
                if (list == null)
                    throw new Exception("No cars found");

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Car>();
            }
        }

        // Get Car by Id
        public async Task<Car> GetById(int carId)
        {
            try
            {
                var car = repo.GetById(carId);
                if (car != null && car.CarId > 0)
                {
                    return car;
                }
                else
                {
                    throw new Exception("Car not found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new Car();
        }

        // Update Car
        public async Task<bool> Update(UpdateCarVM CarVM)
        {
            try
            {
                var imagePath = Upload.UploadFile("Files", CarVM.Car_Image);
                var mappedCar = new Car
                {
                    CarImagePath = imagePath,
                    Brand = CarVM.Brand,
                    Model = CarVM.Model,
                    Year = CarVM.Year,
                    BodyType = CarVM.BodyType,
                    Doors = CarVM.Doors,
                    FuelType = CarVM.FuelType,
                    EngineCapacity = CarVM.EngineCapacity,
                    HorsePower = CarVM.HorsePower,
                    FuelConsumption = CarVM.FuelConsumption,
                    Seats = CarVM.Seats
                };
                if (CarVM != null)
                {
                    var car = repo.GetById(CarVM.CarId); 
                    if (car != null && car.CarId > 0)
                    {
                        mapper.Map(CarVM, car); 
                        repo.Update(car);
                        return true;
                    }
                    else
                    {
                        throw new Exception("Car not found");
                    }
                }
                else
                {
                    throw new Exception("carVM is null");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<bool> RateCar(RateCarVM rateCarVM, string UserId)
        {
            try
            {
                if (rateCarVM == null || UserId == null)
                    return false;

                var car = repo.GetById(rateCarVM.CarId);
                var user = await userManager.FindByIdAsync(UserId);

                if (car != null && car.CarId > 0 && user != null)
                {
                    
                    var existingRate = car.CarRates
                        .FirstOrDefault(r => r.UserId == UserId && r.CarId == rateCarVM.CarId);

                    if (existingRate != null)
                    {
                        
                        existingRate.Rating = rateCarVM.Rating;
                        existingRate.Comment = rateCarVM.Comment;
                        existingRate.UserFullName = user.FullName; 
                    }
                    else
                    {
                        
                        car.CarRates.Add(new CarRate
                        {
                            UserId = UserId,
                            CarId = rateCarVM.CarId,
                            Rating = rateCarVM.Rating,
                            Comment = rateCarVM.Comment,
                            UserFullName = user.FullName
                        });
                    }

                  
                    var ratingSum = car.CarRates.Sum(r => r.Rating);
                    car.AverageRating = (double)ratingSum / car.CarRates.Count;

                    repo.Update(car);
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<Car>> GetAllRandom()
        {
            try
            {
                Random rand = new Random();
                var list = repo.GetAll();
                if (list == null)
                    throw new Exception("No cars found");
                int random = rand.Next(50);
                var RandList = list.OrderBy(i => random).ToList();
                return RandList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Car>();
            }
        }
    }
}

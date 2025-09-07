using AutoMapper;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.CarVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Repo.Abstraction;
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
        public CarService(ICarRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        // Add Car
        public void Add(CreateCarVM carVM)
        {
            try
            {
                if (carVM == null)
                    throw new ArgumentNullException(nameof(carVM));

                var car = mapper.Map<Car>(carVM);
                repo.Add(car);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Delete Car
        public void Delete(int carId)
        {
            try
            {
                var car = repo.GetById(carId);
                if (car != null && car.CarId > 0)
                {
                    repo.Delete(carId);
                }
                else
                {
                    throw new Exception("Car not found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Car ID Not Found!!");
            }
        }

        // Get All Cars
        public List<Car> GetAll()
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
        public Car GetById(int carId)
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
        public void Update(UpdateCarVM carVM)
        {
            try
            {
                if (carVM != null)
                {
                    var car = repo.GetById(carVM.CarId); 
                    if (car != null && car.CarId > 0)
                    {
                        mapper.Map(carVM, car); 
                        repo.Update(car);
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
            }
        }
    }
}

using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.CarVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface ICarService
    {
        public void Add(CreateCarVM user);
        public List<Car> GetAll();
        public void Update(UpdateCarVM user);
        public void Delete(int id);
        public Car GetById(int id);
    }
}

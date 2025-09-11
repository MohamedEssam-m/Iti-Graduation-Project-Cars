using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.MechanicUserVM;
using Cars.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IMechanicService
    {
        public Task Add(CreateMechanicVM user);
        public List<MechanicUser> GetAll();
        public void Update(UpdateMechanicVM user);
        public void Delete(string id);
        public MechanicUser GetById(string id);
    }
}

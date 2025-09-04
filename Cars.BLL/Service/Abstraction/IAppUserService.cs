using Cars.BLL.ModelVM.AppUserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IAppUserService
    {
        public void Add(CreateUserVM user);
        public List<AppUser> GetAll();
        public void Update(UpdateUserVM user);
        public void Delete(string id);
        public AppUser GetById(string id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Repo.Abstraction
{
    public interface IAppUserRepo
    {
        public void Add(AppUser user);
        public List<AppUser> GetAll();
        public void Update(AppUser user);
        public void Delete(string id);
        public AppUser GetById(string id);

    }
}

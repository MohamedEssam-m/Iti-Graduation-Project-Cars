using AutoMapper;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Implementation
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepo repo;
        private readonly IMapper mapper;
        public AppUserService(IAppUserRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        public void Add(CreateUserVM userVM)
        {
            try
            {
                if (userVM == null) 
                    throw new ArgumentNullException("userVM");
                else
                {
                    var user = mapper.Map<AppUser>(userVM);
                    repo.Add(user);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public void Delete(string id)
        {
            try
            {
                var user = repo.GetById(id);
                if(user != null && user.Id != null)
                {
                    repo.Delete(id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("ID Not Found!!");
            }
        }

        public List<AppUser> GetAll()
        {
            try
            {
                List<AppUser> l = repo.GetAll();
                if (l == null)
                    throw new Exception();
                else
                    return l;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("There is no users to show");
            }
            return new List<AppUser>();
        }

        public AppUser GetById(string id)
        {
            try
            {
                var user = repo.GetById(id);
                if (user != null && user.Id != null)
                {
                    return repo.GetById(id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("ID Not Found!!");
            }
            return new AppUser();
        }

        public void Update(UpdateUserVM user)
        {
            try
            {
                if (user != null)
                {
                    var appUser = mapper.Map<AppUser>(user);
                    repo.Update(appUser);
                }
                else
                {
                    throw new Exception("user");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}

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
        public async Task<bool> Add(CreateUserVM userVM)
        {
            try
            {
                if (userVM == null)
                    throw new ArgumentNullException("userVM");
                else
                {
                    var user = mapper.Map<AppUser>(userVM);
                    repo.Add(user);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }


        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                var user = repo.GetById(id);
                if(user != null && user.Id != null)
                {
                    repo.Delete(id);
                    return true;
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
                return false;
            }
        }

        public async Task<List<AppUser>> GetAllUsers()
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

        public async Task<AppUser> GetById(string id)
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

        public async Task<bool> UpdateUser(UpdateUserVM user)
        {
            try
            {
                if (user != null)
                {
                    var appUser = mapper.Map<AppUser>(user);
                    repo.Update(appUser);
                    return true;
                }
                else
                {
                    throw new Exception("user");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}

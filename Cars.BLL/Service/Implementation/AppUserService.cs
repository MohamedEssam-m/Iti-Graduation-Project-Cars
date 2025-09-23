using AutoMapper;
using Cars.BLL.Helper;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> userManager;

        public RoleManager<IdentityRole> RoleManager;
        public AppUserService(IAppUserRepo repo, IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> RoleManager)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.userManager = userManager;
            this.RoleManager = RoleManager;
        }
        public async Task<bool> Add(CreateUserVM userVM)
        {
            try
            {
                //Get User Image_Path
                var imagePath = Upload.UploadFile("Files", userVM.User_Image);
                //var mappedUser = new AppUser
                //{
                    
                //    UserImagePath = imagePath,
                //    FullName = userVM.FullName,
                //    UserName = userVM.Username,
                //    Email = userVM.Email,
                //    PhoneNumber = userVM.PhoneNumber,
                //    Address = userVM.Address,
                //    Age = userVM.Age,
                //};
                if (userVM == null)
                    throw new ArgumentNullException("userVM");
                else
                {
                    var user = mapper.Map<AppUser>(userVM);
                    user.UserImagePath = imagePath;
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
                //Get User Image_Path
                var imagePath = Upload.UploadFile("Files", user.User_Image);
                //var mappedUser = new AppUser
                //{
                //    //هنا عملنا مابنج للصور لان الاوتو مابير مش بيدعم الفورم فايل وكمان ضيفنا اليوزر ايميج عشان نحط فيه المسار
                //    UserImagePath = imagePath,
                //    FullName = user.FullName,
                //    UserName = user.Username,
                //    Email = user.Email,
                //    PhoneNumber = user.PhoneNumber,
                //    Address = user.Address,
                //    Age = user.Age,
                //};
                if (user != null)
                {
                    var appUser = mapper.Map<AppUser>(user);
                    appUser.UserImagePath = imagePath;
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

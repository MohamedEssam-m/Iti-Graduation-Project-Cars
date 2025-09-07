using AutoMapper;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.MechanicUserVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Users;
using Cars.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Implementation
{
    public class MechanicService : IMechanicService
    {
        private readonly IMechanicRepo repo;
        private readonly IMapper mapper;
        public MechanicService(IMechanicRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        //CreateMechanicVM
        //UpdateMechanicVM
        public void Add(CreateMechanicVM mechanicVM)
        {
            try
            {
                if (mechanicVM == null)
                    throw new ArgumentNullException("mechanicVM");
                else
                {
                    var mechanic = mapper.Map<MechanicUser>(mechanicVM);
                    repo.CreateMechanic(mechanic);
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
                var mechanic = repo.GetMechanicById(id);
                if (mechanic != null && mechanic.Id != null)
                {
                    repo.DeleteMechanic(id);
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
        }

        public List<MechanicUser> GetAll()
        {
            try
            {
                List<MechanicUser> l = repo.GetAllMechanics();
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
            return new List<MechanicUser>();
        }

        public MechanicUser GetById(string id)
        {
            try
            {
                var user = repo.GetMechanicById(id);
                if (user != null && user.Id != null)
                {
                    return repo.GetMechanicById(id);
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
            return new MechanicUser();
        }

        public void Update(UpdateMechanicVM mechanic)
        {
            try
            {
                if (mechanic != null)
                {
                    var mechanicuser = mapper.Map<MechanicUser>(mechanic);
                    repo.UpdateMechanic(mechanicuser);
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

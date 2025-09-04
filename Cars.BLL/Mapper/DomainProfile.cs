using AutoMapper;
using Cars.BLL.ModelVM.AppUserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<AppUser, CreateUserVM>().ReverseMap();
        }
    }
}

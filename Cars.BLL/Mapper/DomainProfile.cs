using AutoMapper;
using Cars.BLL.ModelVM.Accident;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.CarVM;
using Cars.BLL.ModelVM.MechanicUserVM;
using Cars.BLL.ModelVM.Offers;
using Cars.BLL.ModelVM.Rent;
using Cars.BLL.ModelVM.RentVM;
using Cars.BLL.ModelVM.Role;
using Cars.DAL.Entities.Accidents;
using Cars.DAL.Entities.Cars;
using Cars.DAL.Entities.Offers;
using Cars.DAL.Entities.Renting;
using Cars.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
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
            CreateMap<AppUser, UpdateUserVM>().ReverseMap();
            CreateMap<Car, CreateCarVM>().ReverseMap();
            CreateMap<Car, UpdateUserVM>().ReverseMap();
            CreateMap<MechanicUser, CreateMechanicVM>().ReverseMap();
            CreateMap<MechanicUser, UpdateMechanicVM>().ReverseMap();
            CreateMap<IdentityRole, CreateRoleVM>().ReverseMap();
            CreateMap<IdentityRole, UpdateRoleVM>().ReverseMap();
            CreateMap<AppUser, SignUpVM>().ReverseMap();
            CreateMap<AppUser, UserWithRoleVM>().ReverseMap();
            CreateMap<Car , CreateCarVM>().ReverseMap();
            CreateMap<Car, UpdateCarVM>().ReverseMap();
            CreateMap<Car, CarRate>().ReverseMap();
            CreateMap<AppUser, AppUser>().ReverseMap();
            CreateMap<Rent, CreateRentVM>().ReverseMap();
            CreateMap<VerifyEmail, ForgetPasswordVM>().ReverseMap();
            CreateMap<UpdateRentVM, Rent>().ReverseMap();
            CreateMap<CreateAccidentVM, Accident>().ReverseMap();
            CreateMap<UpdateAccidentVM, Accident>().ReverseMap();
            CreateMap<UpdateOfferVM, Offer>().ReverseMap();
            CreateMap<CreateOfferVM, Offer>().ReverseMap();

        }
    }
}

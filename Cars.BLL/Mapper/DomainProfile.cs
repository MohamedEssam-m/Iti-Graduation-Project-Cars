using AutoMapper;
using Cars.BLL.ModelVM.CarVM;
using Cars.DAL.Entities.Cars;


namespace Cars.BLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // Car mappings
            CreateMap<Car, CarVM>()
                .ForMember(dest => dest.MadeBy, opt => opt.MapFrom(src => src.Brand)) // Adjust if property names differ
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.OwnerId)); // Adjust if property names differ

            CreateMap<CarVM, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.MadeBy))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<CreateCarVM, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.MadeBy))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<UpdateCarVM, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.MadeBy))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId));

            CreateMap<Car, UpdateCarVM>()
                .ForMember(dest => dest.MadeBy, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.OwnerId));
        }
    }
}
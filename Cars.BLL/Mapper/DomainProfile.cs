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
            CreateMap<MechanicUser, SignUpVM>().ReverseMap();
            CreateMap<Offer, Offer>().ReverseMap();
            CreateMap<GoogleUserData, ProfileCompletionVM>().ReverseMap();
            CreateMap<AppUser, ProfileCompletionVM>().ReverseMap();
            CreateMap<MechanicUser, ProfileCompletionVM>().ReverseMap();
        }
    }
}

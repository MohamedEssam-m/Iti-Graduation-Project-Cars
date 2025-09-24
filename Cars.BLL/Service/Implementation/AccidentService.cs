namespace Cars.BLL.Service.Implementation
{
    public class AccidentService : IAccidentService
    {
        private readonly IAccidentRepo accidentRepo;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public AccidentService(IAccidentRepo accidentRepo, IMapper mapper , UserManager<AppUser> userManager)
        {
            this.accidentRepo = accidentRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> AddAccident(CreateAccidentVM accident , string UserId)
        {
            var imagePath = Upload.UploadFile("Files", accident.Accident_Image);
            //var mappedAccident = new Accident
            //{
            //    AccidentImagePath = imagePath,
            //    Description = accident.Description,
            //    ReportDate = accident.ReportDate,
            //    AccidentDate = accident.AccidentDate,
            //    Location = accident.Location,
            //    carId = accident.carId
            //};

            if (accident == null)
            {
                throw new ArgumentNullException(); 
            }
            var Accident = mapper.Map<Accident>(accident);
            Accident.AccidentImagePath = imagePath;
            Accident.UserId = UserId;
            await accidentRepo.Add(Accident);
            return true;
        }

        public async Task<Accident> GetAccidentById(int id)
        {
            return await accidentRepo.GetById(id);
        }

        public async Task<List<Accident>> GetAllAccidents()
        {
            return await accidentRepo.GetAll();
        }

        public async Task<bool> UpdateAccident(UpdateAccidentVM accident)
        {
            string imagePath = accident.ImagePath; 
            if (accident.Accident_Image != null)
            {
                imagePath = Upload.UploadFile("Files", accident.Accident_Image);
            }
            //var mappedAccident = new Accident
            //{
            //    AccidentImagePath = imagePath,
            //    Description = accident.Description,
            //    ReportDate = accident.ReportDate,
            //    AccidentDate = accident.AccidentDate,
            //    Location = accident.Location,
            //    carId = accident.carId
            //};
            if (accident == null)
            {
                throw new ArgumentNullException(); 
            }
            var Accident = mapper.Map<Accident>(accident);
            Accident.AccidentImagePath = imagePath;
            await accidentRepo.Update(Accident);
            return true;
        }

        public async Task<bool> DeleteAccident(int id)
        {
            await accidentRepo.Delete(id);
            return true;
        }
        
    }
}

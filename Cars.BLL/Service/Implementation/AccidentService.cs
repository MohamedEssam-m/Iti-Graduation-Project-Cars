using AutoMapper;
using Cars.BLL.Helper;
using Cars.BLL.ModelVM.Accident;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Accidents;
using Cars.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var mappedAccident = new Accident
            {
                AccidentImagePath = imagePath,
                Description = accident.Description,
                ReportDate = accident.ReportDate,
                AccidentDate = accident.AccidentDate,
                Location = accident.Location,
                carId = accident.carId
            };

            if (accident == null)
            {
                throw new ArgumentNullException(); 
            }
            var Accident = mapper.Map<Accident>(accident);
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
            var imagePath = Upload.UploadFile("Files", accident.Accident_Image);
            var mappedAccident = new Accident
            {
                AccidentImagePath = imagePath,
                Description = accident.Description,
                ReportDate = accident.ReportDate,
                AccidentDate = accident.AccidentDate,
                Location = accident.Location,
                carId = accident.carId
            };
            if (accident == null)
            {
                throw new ArgumentNullException(); 
            }
            var Accident = mapper.Map<Accident>(accident);
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

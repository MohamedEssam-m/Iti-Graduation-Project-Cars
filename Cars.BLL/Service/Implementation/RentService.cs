using Cars.BLL.Helper.Renting;
using Cars.BLL.ModelVM.Rent;
using Cars.BLL.ModelVM.RentVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Renting;
using Cars.DAL.Repo.Abstraction;
using Cars.DAL.Repo;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Cars.BLL.ModelVM.Payment;

namespace Cars.BLL.Service.Implementation
{
    public class RentService : IRentService
    {
        private readonly IRentRepo _rentRepository;
        private readonly IMapper mapper;

        public RentService(IRentRepo rentRepository, IMapper mapper)
        {
            _rentRepository = rentRepository;
            this.mapper = mapper;
        }

        public async Task<bool> CreateRent(PaymentSummaryVM rentVm)
        {
            // Manual Map CreateRentVM to Rent entity because of nested RentPayment object
            var rent = new Rent
            {
                CarId = rentVm.CarId,
                UserId = rentVm.UserId,
                StartDate = rentVm.StartDate,
                EndDate = rentVm.EndDate,
                Pick_up_location = rentVm.Pick_up_location,
                Drop_Off_location = rentVm.Drop_Off_location,
                Status = "Pending",
                Payment = new RentPayment
                {
                    Amount = rentVm.TotalAmount,
                    IsDone = false,
                    PaymentDate = DateTime.UtcNow
                }
            };

            return await _rentRepository.CreateRent(rent);
        }

        public async Task<bool> DeleteRent(int id)
        {
            var rent = await _rentRepository.GetRentById(id);
            if (rent == null) 
                return false;

            return await _rentRepository.DeleteRent(rent);
        }

        public async Task<Rent> GetRentById(int id)
        {
            return await _rentRepository.GetRentById(id);
        }

        public async Task<List<Rent>> GetAllRents()
        {
            return await _rentRepository.GetAllRents();
        }

        public async Task<bool> UpdateRent(UpdateRentVM rentVm)
        {
            var Rent = await _rentRepository.GetRentById(rentVm.RentId);
            if (Rent == null) 
                return false;

            Rent.StartDate = rentVm.StartDate;
            Rent.EndDate = rentVm.EndDate;
            Rent.Pick_up_location = rentVm.Pick_up_location;
            Rent.Drop_Off_location = rentVm.Drop_Off_location;
            Rent.Status = rentVm.Status;

            if (Rent.Payment != null)
            {
                Rent.Payment.Amount = rentVm.TotalAmount;
                Rent.Payment.IsDone = rentVm.IsDone;
            }

            return await _rentRepository.UpdateRent(Rent);
        }
    }
}

using AutoMapper;
using Cars.BLL.Helper.Renting;
using Cars.BLL.ModelVM.Offers;
using Cars.BLL.ModelVM.Payment;
using Cars.BLL.ModelVM.Rent;
using Cars.BLL.ModelVM.RentVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Renting;
using Cars.DAL.Repo;
using Cars.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cars.BLL.Service.Implementation
{
    public class RentService : IRentService
    {
        private readonly IRentRepo _rentRepository;
        private readonly IMapper mapper;
        private readonly IAppUserService appUserService;
        private readonly ICarService carService;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;
        public RentService(ICarService carService, IConfiguration configuration,IEmailService emailService,IAppUserService appUserService,IRentRepo rentRepository, IMapper mapper)
        {
            _rentRepository = rentRepository;
            this.mapper = mapper;
            this.appUserService = appUserService;
            this.carService = carService;
            this.emailService = emailService;
            this.configuration = configuration;
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
        public async Task CheckRentDate()
        {
            var AllUsers = await appUserService.GetAllUsers();
            foreach (var user in AllUsers)
            {
                if (user.Rents != null && user.Rents.Count > 0)
                {
                    foreach (var rent in user.Rents)
                    {
                        if (rent.Status == "Paid" && rent.EndDate < DateTime.Now)
                        {
                            var car = await carService.GetById(rent.CarId);
                            TimeSpan diff = DateTime.UtcNow - rent.EndDate;
                            rent.Fine = Math.Abs(car.PricePerDay * 2 * ((diff).Minutes));
                            var updateRent = mapper.Map<UpdateRentVM>(rent);
                            updateRent.Fine = rent.Fine;
                            bool result = await UpdateRent(updateRent);
                            if (result)
                            {
                                string subject = $"🚨 Notification of Fine for Late Car Return - Offer #{rent.RentId}";

                                string body = $@"
<div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%); padding: 20px; border-radius: 16px; box-shadow: 0 8px 32px rgba(0,0,0,0.1);'>
    <!-- Header Section -->
    <div style='background: linear-gradient(135deg, #dc2626, #b91c1c); color: white; padding: 25px; border-radius: 12px 12px 0 0; text-align: center; margin: -20px -20px 20px -20px;'>
        <div style='font-size: 48px; margin-bottom: 10px;'>
            🚗⚠️
        </div>
        <h1 style='margin: 0; font-size: 28px; font-weight: 700; text-shadow: 0 2px 4px rgba(0,0,0,0.2);'>
            CAR RETURN REMINDER
        </h1>
        <p style='margin: 8px 0 0 0; font-size: 16px; opacity: 0.9;'>
            Urgent Action Required
        </p>
    </div>

    <!-- Main Content -->
    <div style='background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 16px rgba(0,0,0,0.05); margin-bottom: 20px;'>
        <!-- Greeting -->
        <div style='margin-bottom: 25px;'>
            <h2 style='color: #1e293b; font-size: 24px; font-weight: 600; margin: 0 0 10px 0;'>
                Dear {rent.User?.FullName},
            </h2>
            <p style='color: #64748b; font-size: 16px; line-height: 1.6; margin: 0;'>
                We hope you are doing well.
            </p>
        </div>

        <!-- Alert Box -->
        <div style='background: linear-gradient(135deg, #fef2f2, #fee2e2); border: 2px solid #fecaca; border-left: 6px solid #dc2626; padding: 20px; border-radius: 10px; margin: 25px 0;'>
            <div style='display: flex; align-items: flex-start; gap: 15px;'>
                <div style='font-size: 24px; margin-top: 2px;'>⚠️</div>
                <div>
                    <h3 style='color: #991b1b; font-size: 18px; font-weight: 700; margin: 0 0 8px 0;'>
                        OVERDUE RENTAL NOTICE
                    </h3>
                    <p style='color: #b91c1c; font-size: 15px; line-height: 1.5; margin: 0;'>
                        This is a reminder that the car rental <strong>#{rent.RentId}</strong> associated with your account was scheduled to be returned by <strong>{rent.EndDate:MMM dd, yyyy}</strong>, but it has not been returned yet.
                    </p>
                </div>
            </div>
        </div>

        <!-- Request Section -->
        <div style='background: #f1f5f9; padding: 20px; border-radius: 10px; margin: 20px 0; border-left: 4px solid #06b6d4;'>
            <p style='color: #334155; font-size: 16px; line-height: 1.6; margin: 0; font-weight: 500;'>
                We kindly ask you to <strong style='color: #dc2626;'>return the car as soon as possible</strong> to avoid additional charges or penalties.
            </p>
        </div>

        <!-- Car Details Section -->
        <div style='background: linear-gradient(135deg, #f0f9ff, #e0f2fe); border: 1px solid #bae6fd; padding: 25px; border-radius: 12px; margin: 25px 0;'>
            <h3 style='color: #0c4a6e; font-size: 20px; font-weight: 700; margin: 0 0 15px 0; display: flex; align-items: center; gap: 10px;'>
                🚙 Car Details
            </h3>
            <div style='background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
                <div style='display: grid; gap: 12px;'>
                    <div style='display: flex; justify-content: space-between; align-items: center; padding: 10px 0; border-bottom: 1px solid #e2e8f0;'>
                        <span style='color: #64748b; font-weight: 500;'>Vehicle:</span>
                        <span style='color: #1e293b; font-weight: 700; font-size: 16px;'>{car.Brand} {car.Model}</span>
                    </div>
                    <div style='display: flex; justify-content: space-between; align-items: center; padding: 10px 0;'>
                        <span style='color: #64748b; font-weight: 500;'>Original Return Date:</span>
                        <span style='color: #dc2626; font-weight: 700; font-size: 16px; background: #fef2f2; padding: 4px 12px; border-radius: 20px;'>{rent.EndDate:MMM dd, yyyy}</span>
                    </div>
                </div>
            </div>
        </div>

        <!-- Contact Support Section -->
        <div style='background: linear-gradient(135deg, #f0fdf4, #dcfce7); border: 1px solid #bbf7d0; padding: 20px; border-radius: 10px; margin: 20px 0;'>
            <h4 style='color: #14532d; font-size: 16px; font-weight: 600; margin: 0 0 10px 0; display: flex; align-items: center; gap: 8px;'>
                💬 Need Help?
            </h4>
            <p style='color: #166534; font-size: 14px; line-height: 1.5; margin: 0;'>
                Please contact our support team if you need any assistance regarding the return process or if there are any issues preventing you from returning the car on time.
            </p>
        </div>

        <!-- Appreciation Message -->
        <p style='color: #475569; font-size: 16px; line-height: 1.6; margin: 25px 0 10px 0; text-align: center;'>
            We appreciate your prompt attention to this matter.
        </p>
    </div>

    <!-- Fine Warning Section -->
    <div style='background: linear-gradient(135deg, #fbbf24, #f59e0b); color: white; padding: 20px; border-radius: 12px; text-align: center; box-shadow: 0 4px 16px rgba(251, 191, 36, 0.3);'>
        <div style='font-size: 32px; margin-bottom: 10px;'>💰</div>
        <h3 style='margin: 0 0 8px 0; font-size: 20px; font-weight: 700; text-shadow: 0 2px 4px rgba(0,0,0,0.2);'>
            IMPORTANT NOTICE
        </h3>
        <p style='margin: 0; font-size: 16px; font-weight: 600; opacity: 0.95;'>
            There is a Fine For All Late Days
        </p>
    </div>

    
    <!-- Mobile Responsive Meta -->
    <!--[if mso | IE]>
    <style>
        .email-container 
    </style>
    <![endif]-->
</div>";


                                await emailService.SendEmail(rent.User?.Email, subject, body, configuration["EmailSettings:From"]);
                            }

                        }
                    }
                }
            }
        }
    }
}

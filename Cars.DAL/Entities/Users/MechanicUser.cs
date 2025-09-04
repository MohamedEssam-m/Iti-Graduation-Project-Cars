using Cars.BLL.Helper.Renting;
using Cars.BLL.Helper.Repairing;
using Cars.DAL.Entities.Repairing;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Entities.Users
{
    public class MechanicUser : IdentityUser
    {
        [Required(ErrorMessage = "Name is required, you must add a name.")]
        [Column(TypeName = "nvarchar(30)")]
        public string FullName { get; private set; }

        [Required(ErrorMessage = "Address is required, you must add an address.")]
        [Column(TypeName = "nvarchar(50)")]
        public string Address { get; private set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; private set; }

        [Range(0, 50)]
        public int ExperienceYears { get; private set; }

        [StringLength(200)]
        public string WorkshopAddress { get; private set; }

        public List<Repair> RepairsAsMechanic { get; private set; }
        public List<RentPayment> RentPaymentsAsMechanic { get; private set; }
        public List<RepairPayment> RepairPaymentsAsMechanic { get; private set; }

        public string? ProfilePicture { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public MechanicUser()
        {
            
        }
        public MechanicUser(string fullName, string address, string specialization, int experienceYears, string workshopAddress)
        {
            FullName = fullName;
            Address = address;
            Specialization = specialization;
            ExperienceYears = experienceYears;
            WorkshopAddress = workshopAddress;
            RepairsAsMechanic = new List<Repair>();
            RentPaymentsAsMechanic = new List<RentPayment>();
            RepairPaymentsAsMechanic = new List<RepairPayment>();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}

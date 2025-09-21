using Cars.BLL.Helper.Renting;
using Cars.BLL.Helper.Repairing;
using Cars.DAL.Entities.Offers;
using Cars.DAL.Enums;
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
    public class MechanicUser : AppUser
    {
        public string? MechanicImagePath { get; private set; }
        [Required]
        [StringLength(100)]
        public string Specialization { get; private set; }

        [Range(0, 50)]
        public int ExperienceYears { get; private set; }

        [StringLength(200)]
        public string WorkshopAddress { get; private set; }
        

        public MechanicUser()
        {
            
        }
        public MechanicUser(string fullName, string address, string specialization, int experienceYears, string workshopAddress, string? MechanicImage)
        {
            Specialization = specialization;
            ExperienceYears = experienceYears;
            WorkshopAddress = workshopAddress;
            MechanicImagePath = MechanicImage;
        }
    }
}

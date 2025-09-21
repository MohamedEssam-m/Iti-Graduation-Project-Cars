using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.MechanicUserVM
{
    public class CreateMechanicVM
    {
        public IFormFile Mechanic_Image { get; set; }
        [Required]
        [StringLength(30)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        [StringLength(200)]
        public string WorkshopAddress { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ProfilePicture { get; set; }
    }
}

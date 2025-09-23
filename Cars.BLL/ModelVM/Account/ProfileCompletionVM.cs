using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Account
{
    public class ProfileCompletionVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string EmailVerification { get; set; }
        [Required]
        
        
        public string FullName { get; set; }
        
        //[Required]
        //public string Picture { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Role { get; set; }
        [Required]
        public int Age { get; set; }
        [StringLength(100)]
        public string? Specialization { get;  set; }

        [Range(0, 50)]
        public int? ExperienceYears { get;  set; }

        [StringLength(200)]
        public string? WorkshopAddress { get;  set; }
    }
}

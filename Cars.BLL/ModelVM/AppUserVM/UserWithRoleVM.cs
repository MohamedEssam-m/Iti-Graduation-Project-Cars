using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.AppUserVM
{
    public class UserWithRoleVM
    {
        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(20, ErrorMessage = "Full Name cannot exceed 20 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public List<string> Roleslist { get; set; } = new List<string>();
        [Required]
        public string id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}

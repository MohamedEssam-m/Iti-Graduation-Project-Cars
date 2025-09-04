using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.AppUserVM
{
    public class CreateUserVM
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(30, ErrorMessage = "Full name cannot exceed 30 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(50, ErrorMessage = "Address cannot exceed 50 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }

        public CreateUserVM()
        {
            
        }
    }
}

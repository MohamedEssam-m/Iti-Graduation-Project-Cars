using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.AppUserVM
{
    public class UpdateUserVM
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(30, ErrorMessage = "Full name cannot exceed 30 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(50, ErrorMessage = "Address cannot exceed 50 characters")]
        public string Address { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation do not match")]
        public string? ConfirmPassword { get; set; }
    }
}

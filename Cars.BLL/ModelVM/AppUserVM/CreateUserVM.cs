using Microsoft.AspNetCore.Http;
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
        public IFormFile User_Image { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(30, ErrorMessage = "Full name cannot exceed 30 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "Address cannot exceed 50 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(10, 100, ErrorMessage = "Age must be between 10 and 100.")]
        public int Age { get; set; }

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

        public CreateUserVM(string fullName, string username, string email, string phoneNumber, string address, int age, string password, string confirmPassword)
        {
            FullName = fullName;
            Username = username;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Age = age;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}

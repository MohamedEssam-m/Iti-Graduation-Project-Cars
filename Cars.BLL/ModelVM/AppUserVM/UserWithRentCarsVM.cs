namespace Cars.BLL.ModelVM.AppUserVM
{
    public class UserWithRentCarsVM
    {
        public string? ImagePath { get; set; }
        public IFormFile? Image { get; set; }
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
        [Required(ErrorMessage = "Cars is required")]
        public List<Cars.DAL.Entities.Renting.Rent> Carslist { get; set; } = new List<Cars.DAL.Entities.Renting.Rent>();
        [Required]
        public string id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

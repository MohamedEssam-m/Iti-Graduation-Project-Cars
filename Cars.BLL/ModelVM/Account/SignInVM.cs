namespace Cars.BLL.ModelVM.Account
{
    public class SignInVM
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(30 , ErrorMessage = "The Max Length For UserName Is 30 Characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
}

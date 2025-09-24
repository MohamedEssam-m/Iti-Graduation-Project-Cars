namespace Cars.BLL.ModelVM.Account
{
    public class VerifyEmail
    {
        [EmailAddress(ErrorMessage = "Email Is Required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Token Is Required!")]
        public string Token { get; set; }
    }
}

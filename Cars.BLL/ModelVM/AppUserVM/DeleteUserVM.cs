namespace Cars.BLL.ModelVM.AppUserVM
{
    public class DeleteUserVM
    {
        [Required(ErrorMessage = "User Name Is Required")]
        [MaxLength(20)]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public List<DeleteUserWithRole> UsersList { get; set; } = new List<DeleteUserWithRole>();
    }
}

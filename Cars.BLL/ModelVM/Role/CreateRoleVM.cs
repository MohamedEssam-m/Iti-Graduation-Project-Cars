namespace Cars.BLL.ModelVM.Role
{
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Role Name Is Required!")]
        [MaxLength(20 , ErrorMessage =  "The Name leangth Can not be more than 20 characters!")]
        public string Name { get; set; }
    }
}

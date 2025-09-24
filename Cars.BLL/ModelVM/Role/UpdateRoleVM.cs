namespace Cars.BLL.ModelVM.Role
{
    public class UpdateRoleVM
    {
        [Required(ErrorMessage = "Role Id Is Required!")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name Is Required!")]
        [MaxLength(20, ErrorMessage = "The Name leangth Can not be more than 20 characters!")]
        public string OldName { get; set; }
        [Required(ErrorMessage = "Role Name Is Required!")]
        [MaxLength(20, ErrorMessage = "The Name leangth Can not be more than 20 characters!")]
        public string NewName { get; set; }
        public List<IdentityRole> RolesList { get; set; }
    }
}

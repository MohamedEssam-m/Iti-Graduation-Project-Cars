namespace Cars.BLL.ModelVM.Role
{
    public class DeleteRole
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public List<IdentityRole> RolesList { get; set; }
    }
}

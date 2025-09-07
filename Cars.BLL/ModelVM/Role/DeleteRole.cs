using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

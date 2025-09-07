using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Role
{
    public class CreateRoleVM
    {
        [Required(ErrorMessage = "Role Name Is Required!")]
        [MaxLength(20 , ErrorMessage =  "The Name leangth Can not be more than 20 characters!")]
        public string Name { get; set; }
    }
}

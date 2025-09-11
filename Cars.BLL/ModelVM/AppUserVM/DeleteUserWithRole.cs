using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.AppUserVM
{
    public class DeleteUserWithRole
    {
        [Required]
        public string UserId { get; set; }
        public string RoleName { get; set; }
        [Required(ErrorMessage = "User Name Is Required")]
        [MaxLength(20)]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

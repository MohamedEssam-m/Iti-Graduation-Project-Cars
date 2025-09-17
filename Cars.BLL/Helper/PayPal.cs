using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Helper
{
    public class PayPal
    {
        [Required]
        public string PayPalClientId { get; set; } = "";
        [Required]
        public string PayPalSecret { get; set; } = "";
        [Required]
        public string PayPalUrl { get; set; } = "";
        
    }
}

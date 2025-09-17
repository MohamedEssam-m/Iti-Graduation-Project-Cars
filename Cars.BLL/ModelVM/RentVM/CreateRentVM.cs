using Cars.BLL.Helper.Renting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Rent
{
    public class CreateRentVM
    {
        public int RentId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CarId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string Pick_up_location { get; set; }
        public string Drop_Off_location { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";
        public decimal Amount { get; set; }

    }
}

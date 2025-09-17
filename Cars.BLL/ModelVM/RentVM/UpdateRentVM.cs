using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.RentVM
{
    public class UpdateRentVM
    {
        public int RentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Pick_up_location { get; set; }
        public string Drop_Off_location { get; set; }
        public string Status { get; set; }
        public bool IsDone { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

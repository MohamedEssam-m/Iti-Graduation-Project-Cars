using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.CarVM
{
    public class RateCarVM
    {
        public int CarId { get; set; }
        //public string? UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}

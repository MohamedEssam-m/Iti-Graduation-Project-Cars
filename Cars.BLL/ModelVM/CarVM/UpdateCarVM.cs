using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.CarVM
{
    public class UpdateCarVM
    {
        [Required]
        public int CarId { get; set; } 

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(20, ErrorMessage = "Brand name cannot exceed 20 characters.")]
        public string MadeBy { get; set; }

        [Required(ErrorMessage = "Model of the car is required.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year of the car is required.")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Plate Number is required.")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Owner (User) is required.")]
        public int UserId { get; set; }
        public UpdateCarVM()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Payment
{
    public class OfferPaymentSummaryVM
    {
        [Required]
        public int OfferId { get; set; }

        [Required]
        public int AccidentId { get; set; }

        [Display(Name = "Mechanic Name")]
        [Required(ErrorMessage = "Mechanic name is required")]
        [StringLength(100, ErrorMessage = "Mechanic name cannot exceed 100 characters")]
        public string MechanicName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User name is required")]
        [StringLength(100, ErrorMessage = "User name cannot exceed 100 characters")]
        public string UserName { get; set; }

        [Display(Name = "User Email")]
        [Required(ErrorMessage = "User email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string UserEmail { get; set; }

        [Display(Name = "Repair Start Date")]
        [Required(ErrorMessage = "Repair start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartRepair { get; set; }

        [Display(Name = "Car Receive Date")]
        [Required(ErrorMessage = "Car receive date is required")]
        [DataType(DataType.Date)]
        public DateTime ReceiveCar { get; set; }
        public string  CarName { get; set; }
        public string MechanicEmail { get; set; }
    }
}

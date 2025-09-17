using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Payment
{
    public class PaymentSummaryVM
    {
        [Required(ErrorMessage = "Car Id is required.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Car name is required.")]
        [Display(Name = "Car Name")]
        public string CarName { get; set; }

        [Display(Name = "Car Image")]
        public string CarImage { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [Display(Name = "End Date")]
        
        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 day.")]
        public int Duration { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price per day must be greater than 0.")]
        [Display(Name = "Price Per Day")]
        public decimal PricePerDay { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Invalid currency format.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Pick up location is required.")]
        [Display(Name = "Pick Up Location")]
        public string Pick_up_location { get; set; }

        [Required(ErrorMessage = "Drop off location is required.")]
        [Display(Name = "Drop Off Location")]
        public string Drop_Off_location { get; set; }
    }
}

using Cars.BLL.Helper.Renting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Entities.Renting
{
    public class Rent
    {
        [Key]
        public int RentId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser? User { get; set; }

        [Required(ErrorMessage = "Car is required")]
        public int CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public Car? Car { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid start date format")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid end date format")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Pick-up location is required")]
        [StringLength(100, ErrorMessage = "Pick-up location cannot exceed 100 characters")]
        public string Pick_up_location { get; set; }

        [Required(ErrorMessage = "Drop-off location is required")]
        [StringLength(100, ErrorMessage = "Drop-off location cannot exceed 100 characters")]
        public string Drop_Off_location { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters")]
        public string Status { get; set; } = "Pending";

        public RentPayment? Payment { get; set; }

        //[Required]
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal TotalAmount { get; set; }


    }
}

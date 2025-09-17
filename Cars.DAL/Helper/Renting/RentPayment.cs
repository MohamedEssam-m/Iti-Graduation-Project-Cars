using Cars.DAL.Entities.Renting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Helper.Renting
{
    public class RentPayment
    {
        //public int Id { get; set; }
        [Key]
        [ForeignKey("Rent")]
        public int RentId { get; set; }
        public Rent? Rent { get; set; }

        //[Required]
        //public string UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //public AppUser? User { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Credit Card - PayPal";
        public bool IsDone { get; set; } = false;

        //[Required]
        //[StringLength(50)]
        //public string Status { get; set; }



    }
}

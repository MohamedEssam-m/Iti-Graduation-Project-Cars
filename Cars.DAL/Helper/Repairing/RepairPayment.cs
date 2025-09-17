using Cars.DAL.Entities.Accidents;
using Cars.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Helper.Repairing
{
    public class RepairPayment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int accidentId { get; set; }

        [ForeignKey(nameof(accidentId))]
        public Accident? accident { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        [Required]
        public string? MechanicId { get; set; }  

        [ForeignKey(nameof(MechanicId))]
        public AppUser Mechanic { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PenaltyAmount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }
    }
}

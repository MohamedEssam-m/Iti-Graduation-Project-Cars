using Cars.BLL.Helper.Repairing;
using Cars.DAL.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Entities.Repairing
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }

        [Required]
        public int? CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; }

        [Required]
        public string? MechanicId { get; set; }

        [ForeignKey(nameof(MechanicId))]
        public MechanicUser Mechanic { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime RequestedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProposedPrice { get; set; }

        public DateTime? MechanicResponseDate { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? CompleteDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PenaltyAmount { get; set; }

        public List<RepairPayment> Payments { get; set; }
    }
}

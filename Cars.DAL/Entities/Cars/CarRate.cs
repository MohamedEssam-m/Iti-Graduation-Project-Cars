using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Entities.Cars
{
    public class CarRate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Car is required")]
        [ForeignKey("Car")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "User name is required")]
        [StringLength(100, ErrorMessage = "User name cannot exceed 100 characters")]
        public string? UserFullName { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string Comment { get; set; }

        // Navigation property
        public Car? Car { get; set; }
    }
}

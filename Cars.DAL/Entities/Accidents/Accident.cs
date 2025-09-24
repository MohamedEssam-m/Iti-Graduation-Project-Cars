namespace Cars.DAL.Entities.Accidents
{
    public class Accident
    {
        [Key]
        public int AccidentId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReportDate { get; set; } = DateTime.Now;
        
        [Required(ErrorMessage = "Accident Date Is Required")]
        public DateTime AccidentDate { get; set; }
        [Required(ErrorMessage = "Location ID is required")]
        public string Location { get; set; }
        public string? AccidentImagePath { get; set; }
        [Required(ErrorMessage = "User ID is required")]
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Car")]
        public int carId { get; set; }
        public Car Car { get; set; }
        public AppUser User { get; set; }

        public List<Offer>? Offers { get; set; }
        public RepairPayment? repairPayment { get; set; }
        public string? Status { get; set; } = "Pending";
        
    }
}

namespace Cars.BLL.ModelVM.Accident
{
    public class UpdateAccidentVM
    {
        public int AccidentId { get; set; }
        public IFormFile Accident_Image { get; set; }
        public string? ImagePath { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReportDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Accident Date Is Required")]
        public DateTime AccidentDate { get; set; }
        [Required(ErrorMessage = "Location ID is required")]
        public string Location { get; set; }
        public int carId { get; set; }
        
    }
}

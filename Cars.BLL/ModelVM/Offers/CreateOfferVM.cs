namespace Cars.BLL.ModelVM.Offers
{
    public class CreateOfferVM
    {
        public int CarId { get; set; }
        public string MechanicId { get; set; }
        public int AccidentId { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Details are required")]
        [StringLength(1000, ErrorMessage = "Details cannot exceed 1000 characters")]
        public string Details { get; set; }
        public string CarName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OfferDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "The Starting Date Of Repairing Is Required")]
        public DateTime OfferStartDate { get; set; }
        [Required(ErrorMessage = "The Date Of Recieving The Car Is Required")]
        public DateTime OfferEndDate { get; set; }
    }
}

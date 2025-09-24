namespace Cars.BLL.ModelVM.Offers
{
    public class AllOffersWithUserIdVm
    {
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Details are required")]
        [StringLength(1000, ErrorMessage = "Details cannot exceed 1000 characters")]
        public string Details { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OfferDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "The Starting Date Of Repairing Is Required")]
        public DateTime OfferStartDate { get; set; }
        [Required(ErrorMessage = "The Date Of Recieving The Car Is Required")]
        public DateTime OfferEndDate { get; set; }

        [Required(ErrorMessage = "Car Name Is Required")]
        public string? CarName { get; set; }

        public string UserId { get; set; }

        public AppUser Mechanic { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = "Pending";
    }
}

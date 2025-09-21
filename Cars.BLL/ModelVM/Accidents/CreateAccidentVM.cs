using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Accident
{
    public class CreateAccidentVM
    {
        public IFormFile Accident_Image { get; set; }
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
        public string? AccidentImagePath { get; set; }
    }
}

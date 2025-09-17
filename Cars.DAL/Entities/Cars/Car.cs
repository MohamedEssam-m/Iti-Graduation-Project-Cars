using Cars.DAL.Entities.Accidents;
using Cars.DAL.Entities.Cars;
using Cars.DAL.Entities.Renting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Car
{
    [Key]
    public int CarId { get; private set; }
    public string? CarImagePath { get; private set; }

    [ForeignKey("User")]
    public string? userId { get;  set; }
    public AppUser? User { get; private set; }

    //public List<Repair>? Repairs { get; private set; }
    //new
    public List<Accident>? Accidents { get; private set; } = new List<Accident>();
    public List<Rent>? Rents { get; private set; }
    
    // Brand of the car (e.g., Toyota, BMW, Subaru)
    [Required(ErrorMessage = "Brand is required.")]
    [MaxLength(50, ErrorMessage = "Brand cannot exceed 50 characters")]
    public string Brand { get; set; }

    // Model name of the car (e.g., Elantra, X5)
    [Required(ErrorMessage = "Model is required.")]
    [MaxLength(50, ErrorMessage = "Model cannot exceed 50 character")]
    public string Model { get; set; }

    // Manufacturing year of the car
    [Range(1990, 2100, ErrorMessage = "Year must be between 1990 and 2100")]
    public int Year { get; set; }

    // Type of the car body (Sedan, SUV, Hatchback, etc.)
    [MaxLength(20, ErrorMessage = "Body type cannot exceed 20 characters")]
    public string BodyType { get; set; }

    // Number of doors
    [Range(2, 7, ErrorMessage = "Doors must be between 2 and 7")]
    public int Doors { get; set; }

    // Type of fuel used (Petrol, Diesel, Electric, Hybrid)
    [MaxLength(20, ErrorMessage = "Fuel type cannot exceed 20 characters")]
    public string FuelType { get; set; }

    // Engine displacement in CC (e.g., 1600, 2000)
    [Range(500, 8000, ErrorMessage = "Engine capacity must be between 500 and 8000 CC")]
    public double EngineCapacity { get; set; }

    // Engine power in HorsePower (HP)
    [Range(30, 2000, ErrorMessage = "Horse power must be between 30 and 2000 HP")]
    public int HorsePower { get; set; }

    // Fuel consumption in Km/L
    [Range(1, 50, ErrorMessage = "Fuel consumption must be between 1 and 50 Km/L")]
    public double FuelConsumption { get; set; }

    // Number of passenger seats
    [Range(2, 9, ErrorMessage = "Seats must be between 2 and 9")]
    public int Seats { get; set; }

    // Transmission type (Automatic / Manual)
    [Required(ErrorMessage = "Transmission type is required")]
    public string Transmission { get; set; }

    // Luggage capacity in liters
    [Range(50, 2000, ErrorMessage = "Luggage capacity must be between 50 and 2000 liters")]
    public int LuggageCapacity { get; set; }

    // Rental price per day
    [Required(ErrorMessage = "Price per day is required.")]
    [Range(100, 10000, ErrorMessage = "Price per day must be between 100 and 10000")]
    public decimal PricePerDay { get; set; }

    // Maximum allowed kilometers per day
    [Range(0, 10000, ErrorMessage = "Max kilometers per day must be between 0 and 10000")]
    public int MaxKmPerDay { get; set; }

    // Does the rental include insurance?
    public bool InsuranceIncluded { get; set; }

    // Does the car have air conditioning?
    public bool HasAirCondition { get; set; }

    // Does the car have multimedia system (Bluetooth/Screen)?
    public bool HasMultimedia { get; set; }

    // Safety: Airbags available or not
    public bool HasAirbags { get; set; }

    // Safety: Anti-lock Braking System
    public bool HasABS { get; set; }

    // Safety: Electronic Stability Program
    public bool HasESP { get; set; }

    // Safety: Rear view camera
    public bool HasRearCamera { get; set; }

    // Safety: Parking sensors
    public bool HasParkingSensors { get; set; }

    public double AverageRating { get; set; }
    public List<CarRate>? CarRates { get; set; } = new List<CarRate>();
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100 ")]
    public int quantity { get; set; }
    [Required(ErrorMessage = "Status is Required")]
    public string Status { get; set; }
    public Car()
    {
        Accidents = new List<Accident>();
        Rents = new List<Rent>();
    }

    
   
    
    public bool UpdateCarImage(string imagePath)
    {
        CarImagePath = imagePath;
        return true;
    }


}

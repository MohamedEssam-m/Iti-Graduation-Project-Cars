using Cars.DAL.Entities.Renting;
using Cars.DAL.Entities.Repairing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Car
{
    [Key]
    public int CarId { get; private set; }
    [Required(ErrorMessage ="Brand name is required.")]
    [Column(TypeName ="nvarchar(20)")]
    public string MadeBy { get; private set; }
    [Required(ErrorMessage ="Model of car is required, you must add a model.")]
    public string Model { get; private set; }
    [Required(ErrorMessage ="Year of model's car is required.")]
    public int Year { get; private set; }
    [Required(ErrorMessage ="Plate Number is required.")]
    public string PlateNumber { get; private set; }
    public string CarImagePath { get; private set; }
    [ForeignKey("User")]
    public string? userId { get;  set; }
    public AppUser User { get; private set; }

    public List<Repair> Repairs { get; private set; }
    public List<Rent> Rents { get; private set; }
    


    public Car()
    {
        Repairs = new List<Repair>();
        Rents = new List<Rent>();
    }

    
    public Car(string madeBy, string model, int year, string plateNumber, AppUser user , string CarImagePath)
        
    {
        MadeBy = madeBy;
        Model = model;
        Year = year;
        PlateNumber = plateNumber;
        User = user;
        Repairs = new List<Repair>();
        Rents = new List<Rent>();
        this.CarImagePath = CarImagePath;
    }

    
    public bool UpdateCarImage(string imagePath)
    {
        CarImagePath = imagePath;
        return true;
    }


}


using Cars.BLL.Helper.Renting;
using Cars.BLL.Helper.Repairing;
using Cars.DAL.Entities.Accidents;
using Cars.DAL.Entities.Offers;
using Cars.DAL.Entities.Renting;
using Cars.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class AppUser : IdentityUser
{
    public string? UserImagePath { get; set; }
    [Required(ErrorMessage = "Name is required, you must add a name.")]
    [Column(TypeName = "nvarchar(30)")]
    public string FullName { get;  set; }

    //[Required(ErrorMessage = "Address is required, you must add an address.")]
    [Column(TypeName = "nvarchar(50)")]
    public string? Address { get;  set; }
    
    [Range(18 , 60)]
    public int Age { get;  set; }
    
    
    public List<Car>? Cars { get; private set; } = new List<Car>();
    //public List<Repair>? Repairs { get; private set; }
    public List<Accident>? Accidents { get; private set; } = new List<Accident>();
    // new for mechanics
    public List<Offer>? Offers = new List<Offer>();
    public List<Rent>? Rents { get; private set; } = new List<Rent>();
    public List<RentPayment>? RentPayments { get; private set; } = new List<RentPayment>();
    public List<RepairPayment>? RepairPayments { get; set; } = new List<RepairPayment>();

    
    public bool? IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;
   
    public string? Role { get;  set; }
    

    //public AppUser(string fullName, string address)
    //{
    //    FullName = fullName;
    //    Address = address;
    //    Cars = new List<Car>();
    //    RepairsAsUser = new List<Repair>();
    //    Rents = new List<Rent>();
    //    RentPayments = new List<RentPayment>();
    //    RepairPayments = new List<RepairPayment>();
    //    CreatedAt = DateTime.UtcNow;
    //    IsDeleted = false;
    //}

    public AppUser()
    {
    }

    
    public bool SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        return true;
    }


}

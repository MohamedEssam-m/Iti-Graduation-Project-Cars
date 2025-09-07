
using Cars.BLL.Helper.Renting;
using Cars.BLL.Helper.Repairing;
using Cars.DAL.Entities.Renting;
using Cars.DAL.Entities.Repairing;
using Cars.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class AppUser : IdentityUser
{
    [Required(ErrorMessage = "Name is required, you must add a name.")]
    [Column(TypeName = "nvarchar(30)")]
    public string FullName { get; private set; }

    [Required(ErrorMessage = "Address is required, you must add an address.")]
    [Column(TypeName = "nvarchar(50)")]
    public string Address { get; private set; }
    [Required]
    [Range(18 , 60)]
    public int Age { get; private set; }
    
    
    public List<Car>? Cars { get; private set; }
    public List<Repair>? Repairs { get; private set; }
    public List<Rent>? Rents { get; private set; }
    public List<RentPayment>? RentPayments { get; private set; }
    public List<RepairPayment>? RepairPayments { get;  set; }

    public string? ProfilePicture { get; private set; }
    public bool? IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime? CreatedAt { get; private set; } = DateTime.UtcNow;

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

    public bool UpdateProfile(string fullName, string address, string? profilePicture)
    {
        FullName = fullName;
        Address = address;
        ProfilePicture = profilePicture;
        return true;
    }
    public bool SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        return true;
    }


}

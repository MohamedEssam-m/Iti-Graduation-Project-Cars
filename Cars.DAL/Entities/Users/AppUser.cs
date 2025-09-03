
using Cars.DAL.Entities.Renting;
using Cars.DAL.Entities.Repairing;
using Cars.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class AppUser : IdentityUser
{
    [Required(ErrorMessage ="Name is required, you must add a name.")]
    [Column(TypeName ="nvarchar(30)")]
    public string FullName { get; private set; }
    [Required(ErrorMessage = "Name is required, you must add an address.")]
    [Column(TypeName = "nvarchar(50)")]
    public string Address { get; private set; }
    public List<Car> Cars { get; private set; }
    public MechanicDetails MechanicDetails { get; private set; }
    public List<Repair> RepairsAsCustomer { get; private set; }
    public List<Repair> RepairsAsMechanic { get; private set; }
    public List<Rent> Rents { get; private set; }

    public string? ProfilePicture { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public AppUser(string fullName, string address)
    {
        FullName = fullName;
        Address = address;
        Cars = new List<Car>();
        RepairsAsCustomer = new List<Repair>();
        RepairsAsMechanic = new List<Repair>();
        Rents = new List<Rent>();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
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

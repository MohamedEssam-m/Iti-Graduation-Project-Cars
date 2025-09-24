namespace Cars.DAL.Entities.Users
{
    public class MechanicUser : AppUser
    {
        public string? MechanicImagePath { get; private set; }
        
        [StringLength(100)]
        public string? Specialization { get; private set; }

        [Range(0, 50)]
        public int? ExperienceYears { get; private set; }

        [StringLength(200)]
        public string? WorkshopAddress { get; private set; }
        

        public MechanicUser()
        {
            
        }
        public MechanicUser(string fullName, string address, string specialization, int experienceYears, string workshopAddress, string? MechanicImage)
        {
            Specialization = specialization;
            ExperienceYears = experienceYears;
            WorkshopAddress = workshopAddress;
            MechanicImagePath = MechanicImage;
        }
    }
}

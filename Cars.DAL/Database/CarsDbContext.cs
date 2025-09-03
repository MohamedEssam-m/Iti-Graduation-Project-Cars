using Cars.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Database
{
    public class CarsDbContext :IdentityDbContext<AppUser>
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options)
        {
        }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
    }
}

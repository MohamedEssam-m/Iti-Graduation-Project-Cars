using Cars.BLL.Helper.Renting;
using Cars.BLL.Helper.Repairing;
using Cars.DAL.Entities.Renting;
using Cars.DAL.Entities.Repairing;
using Cars.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.DAL.Database
{
    public class CarsDbContext :IdentityDbContext<AppUser, IdentityRole, string>
    {
        public CarsDbContext(DbContextOptions<CarsDbContext> options) : base(options)
        {
        }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<MechanicUser> MechanicUsers { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<Repair> Repairs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

            // -------------------
            // Car ↔ User
            modelBuilder.Entity<Car>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cars)
                .HasForeignKey(c => c.userId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // -------------------
            // Rent ↔ User
            modelBuilder.Entity<Rent>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rents)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // -------------------
            // Rent ↔ Car
            modelBuilder.Entity<Rent>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Rents)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // -------------------
            // Repair ↔ User
            modelBuilder.Entity<Repair>()
                .HasOne(r => r.User)
                .WithMany(u => u.Repairs)
                .HasForeignKey(r => r.UserId)
                
                .OnDelete(DeleteBehavior.ClientSetNull);

            


            // Repair ↔ Mechanic
            //modelBuilder.Entity<Repair>()
            //    .HasOne(r => r.Mechanic)
            //    .WithMany(u => u.Repairs)
            //    .HasForeignKey(r => r.MechanicId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // -------------------
            // RepairPayment ↔ User
            modelBuilder.Entity<RepairPayment>()
                .HasOne(rp => rp.User)
                .WithMany(u => u.RepairPayments)
                .HasForeignKey(rp => rp.UserId)
                
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<RepairPayment>()
    .HasOne(rp => rp.Repair)
    .WithMany(r => r.Payments)
    .HasForeignKey(rp => rp.RepairId)
    .OnDelete(DeleteBehavior.ClientSetNull);
            //RepairPayment ↔ Mechanic
            //modelBuilder.Entity<RepairPayment>()
            //    .HasOne(rp => rp.Mechanic)
            //    .WithMany(u => u.RepairPayments)
            //    .HasForeignKey(rp => rp.MechanicId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            // -------------------
            // RentPayment ↔ User
            modelBuilder.Entity<RentPayment>()
                .HasOne(rp => rp.User)
                .WithMany(u => u.RentPayments)
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // RentPayment ↔ Rent
            modelBuilder.Entity<RentPayment>()
                .HasOne(rp => rp.Rent)
                .WithMany(r => r.Payments)
                .HasForeignKey(rp => rp.RentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }


    }

}

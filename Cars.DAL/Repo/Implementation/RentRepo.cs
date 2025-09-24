namespace Cars.DAL.Repo.Implementation
{
    
    

    namespace Cars.DAL.Repository
    {
        public class RentRepo : IRentRepo
        {
            private readonly CarsDbContext db;

            public RentRepo(CarsDbContext db)
            {
                this.db = db;
            }

            public async Task<bool> CreateRent(Rent rent)
            {
                await db.Rents.AddAsync(rent);
                return await db.SaveChangesAsync() > 0;
            }

            public async Task<bool> DeleteRent(Rent rent)
            {
                db.Rents.Remove(rent);
                return await db.SaveChangesAsync() > 0;
            }

            public async Task<Rent?> GetRentById(int id)
            {
                return await db.Rents
                    .Include(r => r.Car)
                    .Include(r => r.User)
                    .Include(r => r.Payment)
                    .FirstOrDefaultAsync(r => r.RentId == id);
            }

            public async Task<List<Rent>> GetAllRents()
            {
                return await db.Rents
                    .Include(r => r.Car)
                    .Include(r => r.User)
                    .Include(r => r.Payment)
                    .ToListAsync();
            }

            public async Task<bool> UpdateRent(Rent rent)
            {
                db.Rents.Update(rent);
                return await db.SaveChangesAsync() > 0;
            }
        }
    }

}

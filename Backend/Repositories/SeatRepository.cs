using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly DataContext context;

        public SeatRepository(DataContext context)
        {
            this.context = context;
        }
        
        public async Task<Seat> AddAsync(Seat seat)
        {
            var result = await context.Seats.AddAsync(seat);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Seat> UpdateAsync(Seat seat)
        {
            var result = context.Seats.Update(seat);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var seatToDelete = await context.Seats.FindAsync(id);
            if (seatToDelete == null) return false;
            context.Seats.Remove(seatToDelete);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Seat> GetAsync(int id)
        {
            return await context.Seats
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
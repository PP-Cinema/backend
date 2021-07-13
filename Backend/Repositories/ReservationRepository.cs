using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ReservationRepository: IReservationRepository
    {
        private readonly DataContext context;

        public ReservationRepository(DataContext context) => this.context = context;
        
        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            var result = await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            var result = context.Reservations.Update(reservation);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservationToDelete = await context.Reservations.FindAsync(id);
            if (reservationToDelete == null)
                return false;
            var result = context.Reservations.Remove(reservationToDelete);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Reservation> GetAsync(int id)
        {
            return await context.Reservations.Include(reservation => reservation.Id == id).Include(p => p.Seats).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllUsersReservationsAsync(string email, string lastName)
        {
            return await context.Reservations.Where(r => r.LastName == lastName).Where(r => r.Email == email)
                .Include(r => r.Performance).ThenInclude(p => p.Hall)
                .Include(p => p.Performance).ThenInclude( p => p.Movie)
                .Include(p => p.Seats)
                .Select(r => new Reservation
                {
                    Id = r.Id,
                    NormalTickets = r.NormalTickets,
                    DiscountedTickets = r.DiscountedTickets,
                    Performance = r.Performance
                })
                .ToListAsync();
        }
        
    }
}

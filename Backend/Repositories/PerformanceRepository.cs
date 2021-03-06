using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class PerformanceRepository: IPerformanceRepository
    {
        private readonly DataContext context;

        public PerformanceRepository(DataContext context) => this.context = context;
            
        public async Task<Performance> AddAsync(Performance performance)
        {
            var result = await context.Performances.AddAsync(performance);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Performance> UpdateAsync(Performance performance)
        {
            var result = context.Performances.Update(performance);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Performance> GetAsync(int id)
        {
            return await context.Performances
                .Include(p => p.Hall)
                .Include(p => p.Movie)
                .Include(p => p.Reservations).ThenInclude(r => r.Seats)
                    .FirstOrDefaultAsync(p=> p.Id==id);
        }

        public async Task<Performance> GetAsync(DateTime time, string hall)
        {
            var query = context.Performances
                .Include(p => p.Hall).Include(p => p.Movie)
                .Include(p => p.Reservations).ThenInclude(r => r.Seats)
                .Where(p => p.Date <= time || time.AddMinutes(p.Length) >= p.Date);
            query = query
                .Include(p => p.Hall).Include(p => p.Movie)
                .Where(p => p.Date.AddMinutes(p.Length) >= time);
            return await query
                .Include(p => p.Hall).Include(p => p.Movie)
                .FirstOrDefaultAsync(p => p.Hall.HallLetter == hall);
        }

        public async Task<IEnumerable<Performance>> GetAllAsync()
        {
            return await context.Performances
                .Include(p => p.Hall).Include(p => p.Movie)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var performanceToDelete = await context.Performances.FindAsync(id);
            if (performanceToDelete == null) return false;
            foreach (var reservation in performanceToDelete.Reservations)
            {
                foreach (var seat in reservation.Seats)
                {
                    context.Seats.Remove(seat);
                }
                context.Reservations.Remove(reservation);
            }
            context.Performances.Remove(performanceToDelete);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
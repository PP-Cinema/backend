using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext context;

        public MovieRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            var result = await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Movie> UpdateAsync(Movie movie)
        {
            var result = context.Movies.Update(movie);
            await context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Movie> GetAsync(int id)
        {
            return await context.Movies
                .Include(m=>m.Performances).ThenInclude(p => p.Hall)
                .Include(m=> m.Performances).ThenInclude(p=> p.Reservations)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var temp = await context.Movies
                .Include(m => m.Performances).ThenInclude(p=>p.Hall)
                .OrderBy(m=>m.Title)
                .ToListAsync();
            
            
            foreach (var m in temp)
            {
                var perfList = new List<Performance>();
                perfList.AddRange(m.Performances.Select(p => 
                    new Performance {Id = p.Id, Date = p.Date, Hall = new Hall {HallLetter = p.Hall.HallLetter}}));

                m.Performances = perfList;
            }

            return temp;
        }

        public async Task<Movie> GetAsync(string title)
        {
            return await context.Movies
                .Include(m=>m.Performances).ThenInclude(p => p.Hall)
                .Include(m=> m.Performances).ThenInclude(p=> p.Reservations)
                .FirstOrDefaultAsync(m => m.Title == title);
        }

        public async Task<IEnumerable<Movie>> GetContainsAsync(string title)
        {
            return await context.Movies
                .Include(m=>m.Performances)//.ThenInclude(p => p.Hall)
                //.Include(m=> m.Performances).ThenInclude(p=> p.Reservations)
                .Where(m => m.Title.Contains(title)).ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movieToDelete = await context.Movies.FindAsync(id);
            if (movieToDelete == null) return false;
            foreach (var performance in movieToDelete.Performances)
            {
                foreach (var reservation in performance.Reservations)
                {
                    foreach (var seat in reservation.Seats)
                    {
                        context.Seats.Remove(seat);
                    }
                    context.Reservations.Remove(reservation);
                }
                context.Performances.Remove(performance);
            }
            context.Movies.Remove(movieToDelete);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
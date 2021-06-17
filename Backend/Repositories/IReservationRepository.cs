using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> AddAsync(Reservation reservation);
        Task<Reservation> UpdateAsync(Reservation reservation);
        Task<bool> DeleteAsync(int id);
        Task<Reservation> GetAsync(int id);
    }
}

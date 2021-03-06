using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IHallRepository
    {
        Task<Hall> UpdateAsync(Hall hall);
        Task<Hall> GetAsync(int id);
        Task<Hall> GetAsync(string letterCode);
        Task<IEnumerable<Hall>> GetAllAsync();
    }
}
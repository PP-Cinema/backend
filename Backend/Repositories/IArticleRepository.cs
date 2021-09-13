using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Entities;

namespace Backend.Repositories
{
    public interface IArticleRepository
    {
        Task<Article> AddAsync(Article article);
        Task<Article> UpdateAsync(Article article);
        Task<Article> GetAsync(int id);

        Task<IEnumerable<Article>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
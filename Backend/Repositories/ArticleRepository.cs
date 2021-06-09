using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DataContext context;

        public ArticleRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<Article> AddAsync(Article article)
        {
            var result = await context.Articles.AddAsync(article);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Article> UpdateAsync(Article article)
        {
            var result = context.Articles.Update(article);
            await context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Article> GetAsync(int id)
        {
            return await context.Articles.FindAsync(id);
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await context.Articles.ToListAsync();
        }
    }
}
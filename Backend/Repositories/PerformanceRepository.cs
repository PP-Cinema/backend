﻿using System.Threading.Tasks;
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
            return await context.Performances.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var performanceToDelete = await context.Performances.FindAsync(id);
            if (performanceToDelete == null) return false;
            context.Performances.Remove(performanceToDelete);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
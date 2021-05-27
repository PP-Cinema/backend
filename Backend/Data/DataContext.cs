using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Backend.Entities;

namespace Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
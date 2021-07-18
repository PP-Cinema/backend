using System.Diagnostics.CodeAnalysis;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Admin> Admins { get; set; }
        
        public  DbSet<Seat> Seats { get; set; }
    }
}
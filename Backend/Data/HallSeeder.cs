using System.Collections.Generic;
using System.Linq;
using Backend.Entities;

namespace Backend.Data
{
    public class HallSeeder
    {
        private readonly DataContext context;

        
        public HallSeeder(DataContext context) => this.context = context;
        
        
        public void SeedData()
        {
            if (context.Halls.Any())
                return;
            else
            {
                context.Halls.AddRange(GetHalls());
                context.SaveChanges();
            }
        }

        private ICollection<Hall> GetHalls()
        {
            var halls = new List<Hall>();
            
            halls.Add(new Hall
            {
                Seats = 100
            });
            
            halls.Add(new Hall
            {
                Seats = 120
            });
            
            halls.Add(new Hall
            {
                Seats = 80
            });
            
            halls.Add(new Hall
            {
                Seats = 125
            });
            
            halls.Add(new Hall
            {
                Seats = 130
            });
            
            halls.Add( new Hall
            {
                Seats = 200
            });
            
            halls.Add( new Hall
            {
                Seats = 120
            });
            
            halls.Add( new Hall
            {
                Seats = 200
            });
            
            return halls;
        }
        
    }
}
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
                HallLetter = "A",
                Seats = 100
            });
            
            halls.Add(new Hall
            {
                HallLetter = "B",
                Seats = 120
            });
            
            halls.Add(new Hall
            {
                HallLetter = "C",
                Seats = 80
            });
            
            halls.Add(new Hall
            {
                HallLetter = "D",
                Seats = 125
            });
            
            halls.Add(new Hall
            {
                HallLetter = "E",
                Seats = 130
            });
            
            halls.Add( new Hall
            {
                HallLetter = "F",
                Seats = 200
            });
            
            halls.Add( new Hall
            {
                HallLetter = "G",
                Seats = 120
            });
            
            halls.Add( new Hall
            {
                HallLetter = "H",
                Seats = 200
            });
            
            return halls;
        }
        
    }
}
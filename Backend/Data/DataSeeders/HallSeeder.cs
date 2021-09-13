using System.Collections.Generic;
using System.Linq;
using Backend.Entities;

namespace Backend.Data.DataSeeders
{
    public class HallSeeder
    {
        private readonly DataContext context;


        public HallSeeder(DataContext context)
        {
            this.context = context;
        }


        public void SeedData()
        {
            if (context.Halls.Any()) return;

            context.Halls.AddRange(GetHalls());
            context.SaveChanges();
        }

        private IEnumerable<Hall> GetHalls()
        {
            var halls = new List<Hall>
            {
                new Hall {HallLetter = "A", Seats = 100},
                new Hall {HallLetter = "B", Seats = 120},
                new Hall {HallLetter = "C", Seats = 80},
                new Hall {HallLetter = "D", Seats = 120},
                new Hall {HallLetter = "E", Seats = 130},
                new Hall {HallLetter = "F", Seats = 200},
                new Hall {HallLetter = "G", Seats = 120},
                new Hall {HallLetter = "H", Seats = 200}
            };

            return halls;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Performance
    {
        [Key] public int Id { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public float NormalPrice { get; set; }
        [Required] public float DiscountedPrice { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        [Required] public Hall Hall { get; set; }
        [Required] public Movie Movie { get; set; }
    }
}
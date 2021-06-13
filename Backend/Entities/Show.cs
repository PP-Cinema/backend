using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Show
    {
        [Key] public int Id { get; set; }
        [Required] public DateTime Time { get; set; }
        [Required] public decimal Price { get; set; }
        [Required] public Hall Hall { get; set; }
        [Required] public Movie Movie { get; set; }
    }
}
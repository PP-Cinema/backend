using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Seat
    {
        [Key] public int Id { get; set; }
        
        [Required] public int Row { get; set; }
        
        [Required] public int SeatNumber { get; set; }
    }
}
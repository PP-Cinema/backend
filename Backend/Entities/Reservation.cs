using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Reservation
    {
        [Key] public int Id { get; set; }
        [Required] public string Email { get; set; }
        [Required] public int NormalTickets { get; set; }
        [Required] public int DiscountedTickets { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string Remarks { get; set; }
        [Required] public Performance Performance { get; set; }
    }
}
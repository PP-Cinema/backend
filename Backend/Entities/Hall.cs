using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Hall
    {
        [Key] public int Id { get; set; }
        [Required] public int Seats { get; set; }
        public ICollection<Performance> Performances { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    public class Hall
    {
        [Key] 
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName = "VARCHAR(2)")]
        public string  HallLetter { get; set; }
        
        [Required] 
        public int Seats { get; set; }
        
        public ICollection<Performance> Performances { get; set; }
    }
}
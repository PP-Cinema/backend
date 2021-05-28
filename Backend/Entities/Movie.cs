using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Movie
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public int Length { get; set; }
        public string Description { get; set; }
        public ICollection<Performance> Performances { get; set; }
    }
}
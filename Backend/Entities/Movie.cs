using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities
{
    public class Movie
    {
        [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public int Length { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        [Required]
        public string PosterFilePath { get; set; }
        public ICollection<Performance> Performances { get; set; }
        
        public string TrailerLink { get; set; }
    }
}
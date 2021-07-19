using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Backend.DTO
{
    public class MovieDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Length is required")]
        public int Length { get; set; }
        
        public string Abstract { get; set; }
        
        public string Description { get; set; }
        
        [Required(ErrorMessage = "File is required")]
        public IFormFile PosterFile { get; set; }
        
        public string TrailerLink { get; set; }
    }
}
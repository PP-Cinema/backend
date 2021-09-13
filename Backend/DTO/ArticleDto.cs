using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Backend.DTO
{
    public class ArticleDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Abstract is required")]
        public string Abstract { get; set; }
        
        [Required(ErrorMessage = "Thumbnail is required")]
        public IFormFile ThumbnailFile { get; set; }

        [Required(ErrorMessage = "File is required")]
        public IFormFile File { get; set; }
    }
}
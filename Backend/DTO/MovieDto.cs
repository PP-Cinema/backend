using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class MovieDto
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Length is required")]
        public int Length { get; set; }

        public string Description { get; set; }
    }
}
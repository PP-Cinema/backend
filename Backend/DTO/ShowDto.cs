using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class ShowDto
    {
        [Required(ErrorMessage = "Time is required")]
        public DateTime Time { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Hall is required")]
        public string Hall { get; set; }
        [Required(ErrorMessage = "Movie is required")]
        public string Movie { get; set; }
    }
}
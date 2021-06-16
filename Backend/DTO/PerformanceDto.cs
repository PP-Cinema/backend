using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class PerformanceDto
    {
        [Required(ErrorMessage = "Time is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public float NormalPrice { get; set; }
        [Required(ErrorMessage = "Discounted price is required")]
        public float DiscountedPrice { get; set; }
        [Required(ErrorMessage = "Hall is required")]
        public string Hall { get; set; }
        [Required(ErrorMessage = "Movie is required")]
        public string Movie { get; set; }
    }
}
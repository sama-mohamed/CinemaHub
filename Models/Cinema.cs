using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CinemaWebSite.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]

        public string Description { get; set; }
        [ValidateNever]
        public string? CinemaLogo { get; set; }
        [Required]

        public string Address { get; set; }
        [ValidateNever]
        public ICollection<Movie> Movies { get; set; }
    }
}

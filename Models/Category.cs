using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CinemaWebSite.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; }
        [ValidateNever]
        public ICollection<Movie> Movies { get; set; }
    }
}

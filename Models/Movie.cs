using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CinemaWebSite.Models
{
    public class Movie
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]
        [Range(10, 100)]
        public decimal Price { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }
        [Required]
        public string TrailerUrl { get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        [ValidateNever]
        public int MovieStatus { get; set; }
        public int CinemaId { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual Cinema Cinema { get; set; }
        [ValidateNever]
        public virtual Category Category { get; set; }
        [ValidateNever]
        public ICollection<ActorMovie> ActorMovies { get; set; }

    }


}

using System.ComponentModel.DataAnnotations;

namespace CinemaWebSite.Models
{
    public class Actor
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Bio { get; set; }

        public string ProfilePicture { get; set; }

        public string News { get; set; }

        // Many-to-Many Relationship with Movies
        public ICollection<ActorMovie> ActorMovies { get; set; }
    }
}

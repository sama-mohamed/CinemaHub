using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories.IRepository;

namespace CinemaWebSite.Repositories
{
    public class MovieRepository : Repository<Movie>,IMovieRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories.IRepository;

namespace CinemaWebSite.Repositories
{
    public class ActorRepository : Repository<Actor>,IActorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

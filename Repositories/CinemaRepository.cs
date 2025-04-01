using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories.IRepository;

namespace CinemaWebSite.Repositories
{
    public class CinemaRepository : Repository<Cinema>,ICinemaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

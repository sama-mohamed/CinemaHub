using CinemaWebSite.data;
using CinemaWebSite.Models;
using CinemaWebSite.Repositories.IRepository;

namespace CinemaWebSite.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

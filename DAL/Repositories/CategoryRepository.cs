using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using DAL.Data.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;

namespace DAL.Repositories
{
    public class CategoryRepository : RepoBase<Category>, ICategoryRepository
    {
        public CategoryRepository()
        {

        }

        public CategoryRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}

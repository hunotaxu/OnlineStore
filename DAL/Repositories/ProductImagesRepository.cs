using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductImagesRepository : RepoBase<ProductImages>, IProductImagesRepository
    {
        public ProductImagesRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}

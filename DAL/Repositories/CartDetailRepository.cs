using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CartDetailRepository : BaseRepository<CartDetail>, ICartDetailRepository
    {
        public CartDetailRepository(DbContextOptions<OnlineStoreDbContext> options, OnlineStoreDbContext context = null) : base(options, context)
        {
        }
    }
}
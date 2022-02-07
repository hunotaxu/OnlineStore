using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(DbContextOptions<OnlineStoreDbContext> options, OnlineStoreDbContext context = null) : base(options, context)
        {
        }
    }
}
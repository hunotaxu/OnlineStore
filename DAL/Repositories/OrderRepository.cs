using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderRepository : RepoBase<Order>, IOrderRepository
    {
        public int GetMaxId() => Table.Max(o => o.Id);

        public OrderRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}

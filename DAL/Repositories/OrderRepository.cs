using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public class OrderRepository : RepoBase<Order>, IOrderRepository
    {
        public int GetMaxId() => Table.Max(o => o.Id);

        public PagedResult<Order> GetAllPaging(int pageIndex, int pageSize)
        {
            var query = GetSome(i => i.IsDeleted == false);

            var rowCount = query.ToList().Count;

            query = query.OrderByDescending(x => x.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var paginationSet = new PagedResult<Order>
            {
                Results = query.ToList(),
                CurrentPage = pageIndex,
                RowCount = rowCount,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public OrderRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}

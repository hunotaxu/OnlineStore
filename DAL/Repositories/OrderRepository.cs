using System.Linq;
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public class OrderRepository : RepoBase<Order>, IOrderRepository
    {
        public int GetMaxId() => Table.Max(o => o.Id);

        public PagedResult<Order> GetAllPaging(byte? deliveryType, byte? orderStatus, string keyword, int pageIndex, int pageSize)
        {
            var query = GetSome(i => i.IsDeleted == false);

            if (deliveryType != 0)
            {
                query = query.Where(x => x.DeliveryType == deliveryType.Value);
            }

            if (orderStatus != 0)
            {
                query = query.Where(x => x.Status == orderStatus.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Id.ToString().Contains(keyword, System.StringComparison.OrdinalIgnoreCase));
            }

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

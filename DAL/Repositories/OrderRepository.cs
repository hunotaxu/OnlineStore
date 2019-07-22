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
        public PagedResult<Order> GetAllPaging(byte? deliveryTypeId, byte? orderStatus, string keyword, int pageIndex, int pageSize)
        {
            var query = GetSome(i => i.IsDeleted == false);

            if (deliveryTypeId.HasValue)
            {
                query = query.Where(x => x.ReceivingTypeId == deliveryTypeId.Value);
            }

            if (orderStatus != 0)
            {
                query = query.Where(x => x.Status == (OrderStatus)orderStatus.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Id.ToString().Contains(keyword, System.StringComparison.OrdinalIgnoreCase));
            }

            var rowCount = query.ToList().Count;

            query = query.OrderByDescending(x => x.OrderDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var paginationSet = new PagedResult<Order>
            {
                Results = query.ToList(),
                CurrentPage = pageIndex,
                RowCount = rowCount,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public void AddWithoutSave(Order order)
        {
            Table.Add(order);
        }

        public OrderRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}

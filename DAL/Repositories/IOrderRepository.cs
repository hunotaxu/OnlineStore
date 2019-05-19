using DAL.Data.Entities;
using DAL.Repositories.Base;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public interface IOrderRepository : IRepo<Order>
    {
        int GetMaxId();

        PagedResult<Order> GetAllPaging(byte? deliveryType, byte? orderStatus, string keyword, int pageIndex, int pageSize);
    }
}
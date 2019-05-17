using DAL.Data.Entities;
using DAL.Repositories.Base;
using Utilities.DTOs;

namespace DAL.Repositories
{
    public interface IOrderRepository : IRepo<Order>
    {
        int GetMaxId();

        PagedResult<Order> GetAllPaging(int page, int pageSize);
    }
}
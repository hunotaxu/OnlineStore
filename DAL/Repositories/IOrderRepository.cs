using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public interface IOrderRepository : IRepo<Order>
    {
        int GetMaxId();
    }
}

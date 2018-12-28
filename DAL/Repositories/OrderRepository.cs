using System.Linq;
using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public class OrderRepository : RepoBase<Order>, IOrderRepository
    {
        public int GetMaxId() => Table.Max(o => o.Id);
    }
}

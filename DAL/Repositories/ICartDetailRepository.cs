using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public interface ICartDetailRepository
    {
        int Add(CartDetail entity, bool persist = true);
    }
}

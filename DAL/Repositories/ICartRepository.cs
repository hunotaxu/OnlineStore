using DAL.Data.Entities;
using DAL.Models;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public interface ICartRepository : IRepo<Cart>
    {
        Cart GetCartByCustomerId(int customerId);
        int GetQuantity(int cartId);
        decimal GetTotalAmount(int cartId);
    }
}
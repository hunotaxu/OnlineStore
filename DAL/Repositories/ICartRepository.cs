using System;
using DAL.Data.Entities;
using DAL.Repositories.Base;

namespace DAL.Repositories
{
    public interface ICartRepository : IRepo<Cart>
    {
        Cart GetCartByCustomerId(Guid customerId);
        int GetQuantity(int cartId);
        int Add(Cart cart);
        void DeleteWithoutSave(int cartId);
        decimal GetTotalAmount(int cartId);
    }
}
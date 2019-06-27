using System;
using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CartRepository : RepoBase<Cart>, ICartRepository
    {
        public CartRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }

        public int Add(Cart cart)
        {
            Table.Add(cart);
            SaveChanges();
            return cart.Id;
        }

        public void DeleteWithoutSave(int cartId)
        {
            var cart = Table.FirstOrDefault(x => x.Id == cartId);
            cart.IsDeleted = true;
            Table.Update(cart);
        }

        public Cart GetCartByCustomerId(Guid customerId)
        {
            return Table.FirstOrDefault(c => c.CustomerId == customerId && c.IsDeleted == false);
        }

        public int GetQuantity(int cartId)
        {
            return Db.CartDetail.Where(c => c.CartId == cartId).Sum(n => n.Quantity);
        }

        public decimal GetTotalAmount(int cartId)
        {
            return Find(cartId).CartDetails.Sum(n => n.Item.Price);
        }
    }
}

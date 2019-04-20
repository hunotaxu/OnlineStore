using System;
using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CartRepository : RepoBase<Cart>, ICartRepository
    {
        public CartRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }

        public Cart GetCartByCustomerId(Guid customerId)
        {
            return Table.FirstOrDefault(c => c.CustomerId == customerId);
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

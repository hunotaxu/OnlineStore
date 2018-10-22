using System.Linq;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CartRepository : RepoBase<Cart>, ICartRepository
    {
        public CartRepository()
        {

        }
        public CartRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }

        public Cart GetCartByCustomerId(int customerId)
        {
            return Table.FirstOrDefault(c => c.CustomerId == customerId);
        }

        public int GetQuantity(int cartId)
        {
            return Find(cartId).CartDetail.Sum(n => n.Quantity);
        }

        public decimal GetTotalAmount(int cartId)
        {
            return Find(cartId).CartDetail.Sum(n => n.Item.Price);
        }
    }
}

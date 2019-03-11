using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Data.Entities;

namespace DAL.Repositories
{
    public interface ICartDetailRepository
    {
        CartDetail Find(Expression<Func<CartDetail, bool>> where);
        IEnumerable<CartDetail> GetItems(Expression<Func<CartDetail, bool>> where);
        int Add(CartDetail entity, bool persist = true);
        int Update(CartDetail entity, bool persist = true);
        int Update(int itemId, int cartId, int newQuantity, bool persist = true);
        int Delete(CartDetail entity, bool persist = true);
        int Delete(int itemId, int cartId, bool persist = true);
        int DeleteRange(IEnumerable<CartDetail> entities, bool persist = true);
        int SaveChanges();
    }
}

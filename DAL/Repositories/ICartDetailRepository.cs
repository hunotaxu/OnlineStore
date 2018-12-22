using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public interface ICartDetailRepository
    {
        CartDetail Find(Expression<Func<CartDetail, bool>> where);
        IEnumerable<CartDetail> GetItems(Expression<Func<CartDetail, bool>> where);
        int Add(CartDetail entity, bool persist = true);
        int Update(CartDetail entity, bool persist = true);
        int SaveChanges();
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Models;

namespace DAL.Repositories
{
    public interface ILineItemRepository
    {
        LineItem Find(Expression<Func<LineItem, bool>> where);
        IEnumerable<LineItem> GetItems(Expression<Func<LineItem, bool>> where);
        int Add(LineItem entity, bool persist = true);
        int Update(LineItem entity, bool persist = true);
        int Update(int itemId, int cartId, int newQuantity, bool persist = true);
        int Delete(LineItem entity, bool persist = true);
        int Delete(int itemId, int cartId, bool persist = true);
        int DeleteRange(IEnumerable<LineItem> entities, bool persist = true);
        int SaveChanges();
    }
}

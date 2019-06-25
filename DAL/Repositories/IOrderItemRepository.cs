using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Data.Entities;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IOrderItemRepository
    {
        OrderItem Find(Expression<Func<OrderItem, bool>> where);
        List<OrderItem> GetSome(Expression<Func<OrderItem, bool>> where);
        int Add(OrderItem entity, bool persist = true);
        void AddWithoutSave(OrderItem entity);
        int Update(OrderItem entity, bool persist = true);
        int Update(int itemId, int cartId, int newQuantity, bool persist = true);
        int Delete(OrderItem entity, bool persist = true);
        int DeleteRange(IEnumerable<OrderItem> entities, bool persist = true);
        int Delete(int itemId, int cartId, bool persist = true);
        int SaveChanges();
    }
}

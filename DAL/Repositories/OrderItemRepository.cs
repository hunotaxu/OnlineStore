using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        protected DbSet<OrderItem> Table;
        protected readonly OnlineStoreDbContext Db;

        public OrderItemRepository()
        {

        }
        public OrderItemRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<OrderItem>();
        }

        public OrderItem Find(Expression<Func<OrderItem, bool>> where)
            => Table.FirstOrDefault(where);

        public List<OrderItem> GetSome(Expression<Func<OrderItem, bool>> where) => Table.Where(where).ToList();

        public int Add(OrderItem entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(OrderItem entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Update(int itemId, int orderId, int newQuantity, bool persist = true)
        {
            OrderItem OrderItem = Find(c => c.OrderId == orderId && c.ItemId == itemId);
            Table.Attach(OrderItem).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(OrderItem entity, bool persist = true)
        {
            entity.IsDeleted = true;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(int itemId, int orderId, bool persist = true)
        {
            OrderItem lineItem = Find(c => c.OrderId == orderId && c.ItemId == itemId);
            lineItem.IsDeleted = true;
            Table.Update(lineItem);
            //Table.Attach(OrderItem).State = EntityState.IsDeleted;
            return persist ? SaveChanges() : 0;
        }

        public int DeleteRange(IEnumerable<OrderItem> entities, bool persist = true)
        {
            foreach(var entity in entities)
            {
                entity.IsDeleted = true;
                Table.Update(entity);
            }
            return persist ? SaveChanges() : 0;
        }

        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception ex)
            {
                //Should handle intelligently
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}

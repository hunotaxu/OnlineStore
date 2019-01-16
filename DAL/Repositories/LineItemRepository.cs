using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.EF;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repositories
{
    public class LineItemRepository : ILineItemRepository
    {
        protected DbSet<LineItem> Table;
        protected readonly OnlineStoreDbContext Db;

        public LineItemRepository()
        {

        }
        public LineItemRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<LineItem>();
        }

        public LineItem Find(Expression<Func<LineItem, bool>> where)
            => Table.FirstOrDefault(where);

        public IEnumerable<LineItem> GetItems(Expression<Func<LineItem, bool>> where) => Table.Where(where);

        public int Add(LineItem entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(LineItem entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Update(int itemId, int orderId, int newQuantity, bool persist = true)
        {
            LineItem LineItem = Find(c => c.OrderId == orderId && c.ItemId == itemId);
            Table.Attach(LineItem).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(LineItem entity, bool persist = true)
        {
            entity.Deleted = true;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(int itemId, int orderId, bool persist = true)
        {
            LineItem lineItem = Find(c => c.OrderId == orderId && c.ItemId == itemId);
            lineItem.Deleted = true;
            Table.Update(lineItem);
            //Table.Attach(LineItem).State = EntityState.Deleted;
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

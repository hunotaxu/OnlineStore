using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Data.Entities;

namespace DAL.Repositories
{
    public class CartDetailRepository : ICartDetailRepository
    {
        protected DbSet<CartDetail> Table;
        protected readonly OnlineStoreDbContext Db;

        public CartDetailRepository()
        {

        }
        public CartDetailRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<CartDetail>();
        }

        public CartDetail Find(Expression<Func<CartDetail, bool>> where)
            => Table.FirstOrDefault(where);

        public IEnumerable<CartDetail> GetItems (Expression<Func<CartDetail, bool>> where) => Table.Where(where);

        public int Add(CartDetail entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(CartDetail entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Update(int itemId, int cartId, int newQuantity, bool persist = true)
        {
            CartDetail cartDetail = Find(c => c.CartId == cartId && c.ItemId == itemId);
            Table.Attach(cartDetail).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(CartDetail entity, bool persist = true)
        {
            entity.IsDeleted = true;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(int itemId, int cartId, bool persist = true)
        {
            CartDetail cartDetail = Find(c => c.CartId == cartId && c.ItemId == itemId);
            cartDetail.IsDeleted = true;
            Table.Update(cartDetail);
            //Table.Attach(cartDetail).State = EntityState.IsDeleted;
            return persist ? SaveChanges() : 0;
        }

        public int DeleteRange(IEnumerable<CartDetail> entities, bool persist = true)
        {
            foreach (CartDetail cartDetail in entities)
            {
                cartDetail.IsDeleted = true;
                Table.Update(cartDetail);
            }
            //Table.RemoveRange(entities);
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

using DAL.Data.Entities;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class GoodsReceiptDetailRepository : IGoodsReceiptDetailRepository
    {
        protected DbSet<GoodsReceiptDetail> Table;
        protected readonly OnlineStoreDbContext Db;

        public GoodsReceiptDetailRepository()
        {

        }
        public GoodsReceiptDetailRepository(DbContextOptions<OnlineStoreDbContext> options)
        {
            Db = new OnlineStoreDbContext(options);
            Table = Db.Set<GoodsReceiptDetail>();
        }

        public GoodsReceiptDetail Find(Expression<Func<GoodsReceiptDetail, bool>> where)
            => Table.FirstOrDefault(where);

        public IEnumerable<GoodsReceiptDetail> GetSome(Expression<Func<GoodsReceiptDetail, bool>> where) => Table.Where(where);

        public int Add(GoodsReceiptDetail entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(GoodsReceiptDetail entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Update(int itemId, int GoodsReceiptId, int newQuantity, bool persist = true)
        {
            GoodsReceiptDetail GoodsReceiptDetail = Find(c => c.GoodsReceiptId == GoodsReceiptId && c.ItemId == itemId);
            Table.Attach(GoodsReceiptDetail).State = EntityState.Deleted;
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(GoodsReceiptDetail entity, bool persist = true)
        {
            entity.IsDeleted = true;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public int Delete(int itemId, int GoodsReceiptId, bool persist = true)
        {
            GoodsReceiptDetail GoodsReceiptDetail = Find(c => c.GoodsReceiptId == GoodsReceiptId && c.ItemId == itemId);
            GoodsReceiptDetail.IsDeleted = true;
            Table.Update(GoodsReceiptDetail);
            //Table.Attach(GoodsReceiptDetail).State = EntityState.IsDeleted;
            return persist ? SaveChanges() : 0;
        }

        public int DeleteRange(IEnumerable<GoodsReceiptDetail> entities, bool persist = true)
        {
            foreach (GoodsReceiptDetail GoodsReceiptDetail in entities)
            {
                GoodsReceiptDetail.IsDeleted = true;
                Table.Update(GoodsReceiptDetail);
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

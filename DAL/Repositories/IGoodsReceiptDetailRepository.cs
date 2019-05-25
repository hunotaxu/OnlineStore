using DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public interface IGoodsReceiptDetailRepository
    {
        GoodsReceiptDetail Find(Expression<Func<GoodsReceiptDetail, bool>> where);
        IEnumerable<GoodsReceiptDetail> GetSome(Expression<Func<GoodsReceiptDetail, bool>> where);
        int Add(GoodsReceiptDetail entity, bool persist = true);
        int Update(GoodsReceiptDetail entity, bool persist = true);
        int Update(int itemId, int cartId, int newQuantity, bool persist = true);
        int Delete(GoodsReceiptDetail entity, bool persist = true);
        int Delete(int itemId, int cartId, bool persist = true);
        int DeleteRange(IEnumerable<GoodsReceiptDetail> entities, bool persist = true);
        int SaveChanges();
    }
}

using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GoodsReceiptDetailRepository : BaseRepository<GoodsReceiptDetail>, IGoodsReceiptDetailRepository
    {
        public GoodsReceiptDetailRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}
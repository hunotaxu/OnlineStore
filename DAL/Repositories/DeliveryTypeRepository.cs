using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ReceivingTypeRepository : BaseRepository<ReceivingType>, IReceivingTypeRepository
    {
        public ReceivingTypeRepository(DbContextOptions<OnlineStoreDbContext> options, OnlineStoreDbContext context = null) : base(options, context)
        {
        }
    }
}

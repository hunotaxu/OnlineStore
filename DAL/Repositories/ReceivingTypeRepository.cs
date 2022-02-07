using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ReceivingTypeResponsitory : BaseRepository<ReceivingType>, IReceivingTypeRepository
    {
        public ReceivingTypeResponsitory(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}
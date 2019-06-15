using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ReceivingTypeRepository : RepoBase<ReceivingType>, IReceivingTypeRepository
    {
        public ReceivingTypeRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}

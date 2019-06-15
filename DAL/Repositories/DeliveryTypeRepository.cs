using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DeliveryTypeRepository : RepoBase<DeliveryType>, IDeliveryTypeRepository
    {
        public DeliveryTypeRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}

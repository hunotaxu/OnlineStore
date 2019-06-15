using DAL.Data.Entities;
using DAL.EF;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ShowRoomAddressRepository : RepoBase<ShowRoomAddress>, IShowRoomAddressRepository
    {
        public ShowRoomAddressRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }
    }
}

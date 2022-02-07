using DAL.Data.Entities;
using DAL.EF;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {
        }
    }
}

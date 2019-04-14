using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.EF;
using DAL.Models;

namespace DAL.Repositories
{
    public class UserDecentralizationRepository : IUserDecentralizationRepository
    {
        private readonly OnlineStoreDbContext _context;

        public UserDecentralizationRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        //public IList<UserDecentralization> GetDecentralizations(int? userTypeOfUserId) =>
        //    _context.Set<UserDecentralization>().Where(u => u.TypeOfUserId == userTypeOfUserId).ToList();
    }
}

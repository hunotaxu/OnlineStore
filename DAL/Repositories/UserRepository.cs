using DAL.Repositories.Base;
using DAL.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : RepoBase<User>, IUserRepository
    {
        public UserRepository()
        {

        }

        public UserRepository(DbContextOptions<OnlineStoreDbContext> options) : base(options)
        {

        }

        public User GetUserByUsername(string username)
        {
            return Table.SingleOrDefault(n => n.Username == username);
        }

        public User GetUser(string username, string password)
        {
            return Table.SingleOrDefault(n => n.Username == username && n.Password == password);
        }
    }
}
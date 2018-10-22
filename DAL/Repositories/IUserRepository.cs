using DAL.Repositories.Base;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository : IRepo<User>
    {
        User GetUser(string username, string password);
    }
}

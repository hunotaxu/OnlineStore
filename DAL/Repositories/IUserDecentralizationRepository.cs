using System.Collections.Generic;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserDecentralizationRepository
    {
        IList<UserDecentralization> GetDecentralizations(int? userTypeOfUserId);
    }
}

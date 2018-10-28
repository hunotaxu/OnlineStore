using DAL.Repositories.Base;
using DAL.Models;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IItemRepository : IRepo<Item>
    {
        IList<Item> GetByCategory(int categoryId);
        Item GetItem(int? id);
        IEnumerable<Item> GetItemByName(string name);
    }
}

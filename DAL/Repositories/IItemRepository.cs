using DAL.Repositories.Base;
using DAL.Models;
using System.Collections.Generic;
using DAL.Data.Entities;

namespace DAL.Repositories
{
    public interface IItemRepository : IRepo<Item>
    {
        IEnumerable<Item> GetByCategory(int categoryId);
        Item GetItem(int? id);
        IEnumerable<Item> GetItemByName(string name);
    }
}

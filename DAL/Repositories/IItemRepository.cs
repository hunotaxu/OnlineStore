using DAL.Repositories.Base;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public interface IItemRepository : IRepo<Item>
    {
        IList<Item> GetByCategory(int categoryId);
    }
}

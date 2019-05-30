using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Interfaces
{
    public interface IItemService : IDisposable
    {
        ItemViewModel GetById(int id);
    }
}

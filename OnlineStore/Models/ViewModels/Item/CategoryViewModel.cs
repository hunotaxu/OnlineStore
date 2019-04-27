using System;
using System.Collections.Generic;

namespace OnlineStore.Models.ViewModels.Item
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public ICollection<ItemViewModel> Items { get; set; }
    }
}

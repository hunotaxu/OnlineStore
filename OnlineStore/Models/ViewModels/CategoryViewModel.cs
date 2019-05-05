using System;
using System.Collections.Generic;
using DAL.Data.Enums;

namespace OnlineStore.Models.ViewModels.Item
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte? Status { set; get; }

        public int? SortOrder { set; get; }

        public int? ParentId { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public ICollection<ItemViewModel> Items { get; set; }
    }
}
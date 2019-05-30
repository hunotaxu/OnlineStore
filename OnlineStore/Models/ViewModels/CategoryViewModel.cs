using System;
using System.Collections.Generic;
using DAL.Data.Enums;
using OnlineStore.Models.ViewModels.Item;

namespace OnlineStore.Models.ViewModels
{ 
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte? Status { set; get; }

        public byte? SortOrder { set; get; }

        public int? ParentId { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateModified { set; get; }

        public ICollection<ItemViewModel> Items { get; set; }
    }
}
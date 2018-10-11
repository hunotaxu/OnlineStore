using DAL.Models.Base;
using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class Category : EntityBase
    {
        public Category()
        {
            Item = new HashSet<Item>();
        }

        //public int Id { get; set; }
        public string Name { get; set; }
        //public byte[] Timestamp { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}

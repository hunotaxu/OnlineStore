using DAL.Models.Base;
using System.Collections.Generic;

namespace DAL.Models
{
    public class Category : EntityBase
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

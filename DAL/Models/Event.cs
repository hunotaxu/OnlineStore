using System;
using System.Collections.Generic;

namespace OnlineStore.Models
{
    public partial class Event
    {
        public Event()
        {
            Item = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public decimal? Bonus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte[] Timestamp { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}

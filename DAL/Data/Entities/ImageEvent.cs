using DAL.Models.Base;

namespace DAL.Models
{
    public class ImageEvent : EntityBase
    {
        public string Directory { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class ImageEvent : EntityBase
    {
        public string Directory { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

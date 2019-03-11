using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class ImageProduct : EntityBase
    {
        public string Directory { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }
    }
}

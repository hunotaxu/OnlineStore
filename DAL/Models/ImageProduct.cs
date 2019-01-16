using DAL.Models.Base;

namespace DAL.Models
{
    public class ImageProduct : EntityBase
    {
        public string Directory { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }
    }
}

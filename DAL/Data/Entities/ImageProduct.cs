using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class ImageProduct : EntityBase
    {
        public string Url { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
    }
}

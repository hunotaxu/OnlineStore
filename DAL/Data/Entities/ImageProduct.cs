using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class ProductImages : EntityBase
    {
        public string Url { get; set; }

        public string Caption { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
    }
}

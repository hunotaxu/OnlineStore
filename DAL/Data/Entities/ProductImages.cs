using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class ProductImages : EntityBase
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
    }
}

using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class CartDetail : EntityBase
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Item Item { get; set; }
    }
}
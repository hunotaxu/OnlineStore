using DAL.Models.Base;

namespace DAL.Models
{
    public class Comment : EntityBase
    {
        //public int Id { get; set; }
        public string Content { get; set; }
        public int? ItemId { get; set; }
        public int? Evaluation { get; set; }

        public Item Item { get; set; }
    }
}

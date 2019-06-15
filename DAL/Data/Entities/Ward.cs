using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Ward : EntityBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string LatiLongTude { get; set; }
        public int DistrictId { get; set; }
        public int SortOrder { get; set; }
        public bool IsPublished { get; set; }

    }
}

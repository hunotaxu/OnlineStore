using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class District : EntityBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string LatiLongTude { get; set; }
        public int ProvinceId { get; set; }
        public int SortOrder { get; set; }
        public bool IsPublished { get; set; }

    }
}

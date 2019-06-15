using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class Province :  EntityBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int TelephoneCode { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public int CountryId { get; set; }
        public int SortOrder { get; set; }
        public bool IsPublished { get; set; }

    }
}

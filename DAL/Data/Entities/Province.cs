using DAL.Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class Province : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
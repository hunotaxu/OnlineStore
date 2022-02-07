using DAL.Data.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data.Entities
{
    public class Ward : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string LatiLongTude { get; set; }
        public int DistrictId { get; set; }
        public int SortOrder { get; set; }
        public bool IsPublished { get; set; }
    }
}
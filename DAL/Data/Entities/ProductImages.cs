using DAL.Data.Entities.Base;
using Newtonsoft.Json;

namespace DAL.Data.Entities
{
    public class ProductImages : EntityBase
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int ItemId { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; }
        public string ContentType { get; set; }
        public byte[] Contents { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DAL.Data.Entities.Base
{
    public class EntityBase
    {
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public bool IsDeleted { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Base
{
    public class EntityBase
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int ID { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
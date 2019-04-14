using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Data.Entities.Base;

namespace DAL.Data.Entities
{
    public class TypeOfCustomer : EntityBase
    {
        public TypeOfCustomer()
        {
            Customers = new HashSet<Customer>();
        }

        [Required]
        [DisplayName("Tên loại khách hàng")]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}

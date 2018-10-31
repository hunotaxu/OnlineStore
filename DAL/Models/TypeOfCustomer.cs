using System.Collections.Generic;
using DAL.Models.Base;

namespace DAL.Models
{
    public class TypeOfCustomer : EntityBase
    {
        public TypeOfCustomer()
        {
            Customers = new HashSet<Customer>();
        }

        public string Name { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}

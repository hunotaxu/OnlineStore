using DAL.Data.Entities.Base;
using System;

namespace DAL.Data.Entities
{
    public class DeliveryType : EntityBase
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}

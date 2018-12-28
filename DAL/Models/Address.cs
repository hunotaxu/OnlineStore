using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models.Base;

namespace DAL.Models
{
    public class Address : EntityBase
    {
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Detail { get; set; }
        public int CustomerId { get; set; }
        public string PhoneNumber { get; set; }

        public Customer Customer { get; set; }
    }
}

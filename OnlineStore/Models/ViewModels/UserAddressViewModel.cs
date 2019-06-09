using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class UserAddressViewModel
    { 
        public Guid CustomerId { get; set; }
        public int AddressId { get; set; }
        public bool IsDeleted { get; set; }
        public string PhoneNumber { get; set; }
        public string RecipientName { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Detail { get; set; }
    }
}

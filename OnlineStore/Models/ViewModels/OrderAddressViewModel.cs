using DAL.Data.Entities;

namespace OnlineStore.Models.ViewModels
{
    public class OrderAddressViewModel
    { 
        public Order Order { get; set; }
        public Address Address { get; set; }
    }
}

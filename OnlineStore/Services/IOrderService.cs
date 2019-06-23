using DAL.Data.Entities;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Services
{
    public interface IOrderService
    {
        bool SaveOrder(OrderAddressViewModel model, ApplicationUser user, out string error);
    }
}

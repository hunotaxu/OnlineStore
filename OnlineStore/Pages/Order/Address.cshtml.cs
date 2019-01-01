using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Extensions;

namespace OnlineStore.Pages.Order
{
    public class AddressModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private ICartDetailRepository _cartDetailRepository;
        private ICartRepository _cartRepository;
        private IItemRepository _itemRepository;
        private readonly IAddressRepository _addressRepository;

        public AddressModel(IOrderRepository orderRepository, ICartDetailRepository cartDetailRepository,
            ICartRepository cartRepository, IItemRepository itemRepository, IAddressRepository addressRepository)
        {
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _itemRepository = itemRepository;
            ShippingFeeLocal = 10000;
            ShippingFeeGlobal = 30000;
        }

        public Customer Customer { get; set; }
        public IList<Address> AddressOfCustomer { get; set; }
        public decimal ShippingFeeLocal { get; set; }
        public decimal ShippingFeeGlobal { get; set; }

        public IActionResult OnGet(int cartId)
        {

            //decimal a = _cartDetailRepository.GetItems(c => c.CartId == cartId).Sum(x => x.Quantity);
            Customer = HttpContext.Session.Get<Customer>("Customer");
            if (Customer == null)
            {
                return RedirectToPage("/Account/Login");
            }
            AddressOfCustomer = _addressRepository.GetSome(c => c.CustomerId == Customer.Id).ToList();
            return Page();
        }

        public IActionResult OnPostAddress(Address address)
        {
            Customer = HttpContext.Session.Get<Customer>("Customer");
            if (Customer == null)
            {
                return RedirectToPage("/Account/Login");
            }
            if (address.Id != 0)
            {
                Address add = _addressRepository.Find(address.Id);
                add.District = address.District;
                add.Detail = address.Detail;
                add.PhoneNumber = address.PhoneNumber;
                add.Province = address.Province;
                add.Ward = address.Ward;
                _addressRepository.Update(add);
                return RedirectToPage("./Address");
            }

            address.CustomerId = Customer.Id;
            _addressRepository.Add(address);

            return RedirectToPage("./Address");
        }

        public IActionResult OnGetDeleteAddress(int addressId)
        {
            Address address = _addressRepository.Find(addressId);
            if (address != null)
            {
                _addressRepository.Delete(address);
            }
            return RedirectToPage("./Address");
        }

        //public IActionResult OnGetOrder()
        //{
        //    DAL.Models.Order order = new DAL.Models.Order()
        //    {
        //        CustomerId = Customer.Id,
        //        OrderDate = DateTime.Now,
        //        Status = (int)StatusOrder.Pending
        //    };
        //    _orderRepository.Add(order);
        //    int MaxId = _orderRepository.GetMaxId();
        //    LineItem lineItem = new LineItem()
        //    {
        //        OrderId = MaxId,
        //    };
        //    DAL.Models.Cart cart = _cartRepository.Find(c => c.CustomerId == Customer.Id);
        //    if (cart != null)
        //    {
        //        IEnumerable<CartDetail> cartDetails = _cartDetailRepository.GetItems(c => c.CartId == cart.Id);
        //        foreach (var cartItem in cartDetails)
        //        {
        //            lineItem.ItemId = cartItem.ItemId;
        //            lineItem.Quantity = cartItem.Quantity;
        //            lineItem.Amount = _itemRepository.Find(cartItem.ItemId).Price * cartItem.Quantity;
        //        }
        //    }

        //}
    }
}
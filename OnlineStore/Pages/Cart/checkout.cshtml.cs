using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using OnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DAL.Data.Enums;
using DAL.EF;
using OnlineStore.Services;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        //private readonly IUserAddressRepository _userAddressRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IDefaultAddressRepository _defaultAddressRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        public readonly IWardRepository _wardRepository;
        public readonly IAddressRepository _addressRepository;
        public readonly IOrderItemRepository _orderItemRepository;
        public readonly IOrderRepository _orderRepository;
        public readonly IShowRoomAddressRepository _showRoomAddressRepository;
        public readonly IReceivingTypeRepository _receivingTypeRepository;
        public readonly IUnitOfWork _unitOfWork;
        protected readonly DbContextOptions<OnlineStoreDbContext> _options;

        public CheckoutModel(IReceivingTypeRepository receivingTypeRepository, IShowRoomAddressRepository showRoomAddressRepository, IAddressRepository addressRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, ICartDetailRepository cartDetailRepository,
            IUserRepository userRepository,
            IDefaultAddressRepository defaultAddressRepository,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            DbContextOptions<OnlineStoreDbContext> options)
        {
            _options = options;
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _defaultAddressRepository = defaultAddressRepository;
            _userRepository = userRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _addressRepository = addressRepository;
            _showRoomAddressRepository = showRoomAddressRepository;
            _receivingTypeRepository = receivingTypeRepository;
        }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }
        [BindProperty]
        public List<UserAddressViewModel> UserAddresses { get; set; }
        [BindProperty]
        public UserAddressViewModel DefaultAddress { get; set; }
        [BindProperty]
        public List<Province> Provinces { get; set; }
        [BindProperty]
        public List<District> Districts { get; set; }
        [BindProperty]
        public List<Ward> Wards { get; set; }
        [BindProperty]
        public List<ShowRoomAddress> Showrooms { get; set; }
        [BindProperty]
        public List<ReceivingType> ReceivingTypes { get; set; }

        [BindProperty]
        public List<DAL.Data.Entities.Order> Orders { get; set; }

        public int OrderId { get; set; }

        public ActionResult OnGet()
        {
            //var user = _userManager.GetUserAsync(HttpContext.User).Result;
            //if (user == null || _userRepository.IsAdmin(user))
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Cart/Index" });
            //}
            return Page();
        }

        public IActionResult OnGetLoadTmpCart()
        {
            var cart = _cartRepository.GetCartByCustomerId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            if (cart != null)
            {
                ItemInCarts = new List<ItemCartViewModel>();
                var items = cart.CartDetails.Where(cd => cd.IsDeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        //if (item.Item.Quantity == 0 || item.Item.Quantity < item.Quantity)
                        //{
                        //    return new BadRequestObjectResult("Số lượng sản phẩm của cửa hàng không còn đủ cho đơn hàng. Vui lòng xác nhận lại giỏ hàng.");
                        //}
                        if (item.Item.Quantity != 0 && item.Item.Quantity >= item.Quantity)
                        {
                            var itemCartViewModel = new ItemCartViewModel
                            {
                                ItemId = item.ItemId,
                                Image = $"/images/client/ProductImages/{item.Item.ProductImages?.FirstOrDefault()?.Name}",
                                Price = item.Item.Price,
                                ProductName = item.Item.Name,
                                //Quantity = item.Quantity < item.Item.Quantity || item.Item.Quantity == 0) ? item.Quantity : item.Item.Quantity,
                                Quantity = item.Quantity,
                                //MaxQuantity = item.Item.Quantity
                            };
                            ItemInCarts.Add(itemCartViewModel);
                        }
                    }
                }
            }
            return new OkObjectResult(ItemInCarts);
        }
        public IActionResult OnGetLoadDefaultAddress()
        {
            //var useraddress = _userAddressRepository.GetByUserId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            var address = _addressRepository.GetSome(x => x.CustomerId == _userManager.GetUserAsync(HttpContext.User).Result.Id && x.IsDeleted == false);
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user != null)
            {
                var defaultAddress = _defaultAddressRepository.GetSome(x => x.CustomerId == user.Id && x.IsDeleted == false).FirstOrDefault();
                if (defaultAddress != null)
                {
                    //UserAddresses = new List<UserAddressViewModel>();
                    //var items = useraddress.Where(cd => cd.IsDeleted == false).ToList();
                    //if (items.Count > 0)
                    //{
                    //foreach (var item in address)
                    //{
                    DefaultAddress = new UserAddressViewModel
                    {
                        AddressId = defaultAddress.Address.Id,
                        CustomerId = defaultAddress.Address.CustomerId,
                        PhoneNumber = defaultAddress.Address.PhoneNumber,
                        RecipientName = defaultAddress.Address.RecipientName,
                        Province = defaultAddress.Address.Province,
                        District = defaultAddress.Address.District,
                        Ward = defaultAddress.Address.Ward,
                        Detail = defaultAddress.Address.Detail
                    };
                    //UserAddresses.Add(userAddress);
                }
                //}
            }
            //if (address != null && address.Count() > 0)
            //{
            //    UserAddresses = new List<UserAddressViewModel>();
            //    //var items = useraddress.Where(cd => cd.IsDeleted == false).ToList();
            //    //if (items.Count > 0)
            //    //{
            //    foreach (var item in address)
            //    {
            //        var userAddress = new UserAddressViewModel
            //        {
            //            AddressId = item.Id,
            //            CustomerId = item.CustomerId,
            //            PhoneNumber = item.PhoneNumber,
            //            RecipientName = item.RecipientName,
            //            Province = item.Province,
            //            District = item.District,
            //            Ward = item.Ward,
            //            Detail = item.Detail
            //        };
            //        UserAddresses.Add(userAddress);
            //    }
            //}
            return new OkObjectResult(DefaultAddress);
        }
        public IActionResult OnGetLoadAddress(int? availableAddressId)
        {
            //var useraddress = _userAddressRepository.GetByUserId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            var customer = _userManager.GetUserAsync(HttpContext.User).Result;
            if (customer != null)
            {
                //var defaultAddress = _defaultAddressRepository.GetSome(x => x.CustomerId == customer.Id);
                var address = _addressRepository.GetSome(x => x.CustomerId == customer.Id && x.IsDeleted == false && x.ShowRoomAddressId == null);
                if (address != null && address.Count() > 0)
                {
                    UserAddresses = new List<UserAddressViewModel>();
                    //var items = useraddress.Where(cd => cd.IsDeleted == false).ToList();
                    //if (items.Count > 0)
                    //{```````
                    foreach (var item in address)
                    {
                        var userAddress = new UserAddressViewModel
                        {
                            AddressId = item.Id,
                            CustomerId = item.CustomerId,
                            PhoneNumber = item.PhoneNumber,
                            RecipientName = item.RecipientName,
                            Province = item.Province,
                            District = item.District,
                            Ward = item.Ward,
                            Detail = item.Detail,
                            DefaultChecked = availableAddressId != null && availableAddressId > 0 && availableAddressId == item.Id ? "checked" : ""
                            //DefaultChecked = item.Id == defaultAddress?.First().AddressId ? "checked" : ""
                        };
                        //if (defaultAddress != null)
                        //{
                        //    if (userAddress.AddressId == defaultAddress.AddressId)
                        //    {
                        //        userAddress.DefaultChecked = "checked";
                        //    }
                        //}
                        UserAddresses.Add(userAddress);
                    }
                }
            }
            return new OkObjectResult(UserAddresses);
        }

        public IActionResult OnGetLoadProvince()
        {
            var province = _provinceRepository.GetAll();
            if (province != null)
            {
                Provinces = new List<Province>();
                var items = province.Where(cd => cd.IsDeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var provinces = new Province
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Type = item.Type,
                            TelephoneCode = item.TelephoneCode,
                            ZipCode = item.ZipCode,
                            CountryId = item.CountryId,
                            SortOrder = item.SortOrder,
                            IsPublished = item.IsPublished,
                            IsDeleted = item.IsDeleted,
                            Timestamp = item.Timestamp,
                        };
                        Provinces.Add(provinces);
                    }
                }
            }
            return new OkObjectResult(Provinces);
        }



        public IActionResult OnPostLoadDistrict([FromBody] District model)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestResult();
            }

            var district = _districtRepository.GetSome(cd => cd.ProvinceId == model.ProvinceId);
            if (district != null)
            {
                Districts = new List<District>();
                var items = district.Where(cd => cd.IsDeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var districts = new District
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Type = item.Type,
                            SortOrder = item.SortOrder,
                            LatiLongTude = item.LatiLongTude,
                            ProvinceId = item.ProvinceId,
                            IsPublished = item.IsPublished,
                            IsDeleted = item.IsDeleted,
                            Timestamp = item.Timestamp,
                        };
                        Districts.Add(districts);
                    }
                }
            }
            return new OkObjectResult(Districts);
        }

        public IActionResult OnPostLoadWard([FromBody] Ward model)
        {

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestResult();
            }

            var ward = _wardRepository.GetSome(cd => cd.DistrictId == model.DistrictId);
            if (ward != null)
            {
                Wards = new List<Ward>();
                var items = ward.Where(cd => cd.IsDeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var wards = new Ward
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Type = item.Type,
                            SortOrder = item.SortOrder,
                            LatiLongTude = item.LatiLongTude,
                            DistrictId = item.DistrictId,
                            IsPublished = item.IsPublished,
                            IsDeleted = item.IsDeleted,
                            Timestamp = item.Timestamp,
                        };
                        Wards.Add(wards);
                    }
                }
            }
            return new OkObjectResult(Wards);
        }

        public IActionResult OnPostSaveAddress([FromBody] UserAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            //if (user != null && !_userRepository.IsAdmin())
            //if (user != null && !HttpContext.User.IsInRole(CommonConstants.CustomerRoleName))
            //{

            var newAddress = new Address
            {
                PhoneNumber = model.PhoneNumber,
                RecipientName = model.RecipientName,
                Ward = model.Ward,
                District = model.District,
                Province = model.Province,
                Detail = model.Detail,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                CustomerId = _userManager.GetUserAsync(HttpContext.User).Result.Id
            };
            _addressRepository.Add(newAddress);
            //_userAddressRepository.Add(new UserAddress
            //{
            //    CustomerId = _userManager.GetUserAsync(HttpContext.User).Result.Id,
            //    AddressId = newAddress.Id,
            //    PhoneNumber = model.PhoneNumber,
            //    RecipientName = model.RecipientName,
            //});
            //}
            return new OkObjectResult(model);
        }
        public IActionResult OnGetLoadShowroom()
        {
            var showroom = _showRoomAddressRepository.GetAll();
            if (showroom != null)
            {
                Showrooms = new List<ShowRoomAddress>();
                var items = showroom.Where(cd => cd.IsDeleted == false).ToList();

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var showrooms = new ShowRoomAddress
                        {
                            Id = item.Id,
                            Province = _provinceRepository.Find(cd => cd.Id == item.ProvinceId && cd.IsDeleted == false),
                            District = _districtRepository.Find(cd => cd.Id == item.DistrictId && cd.IsDeleted == false),
                            Ward = _wardRepository.Find(cd => cd.Id == item.WardId && cd.IsDeleted == false),
                            Detail = item.Detail,
                        };
                        Showrooms.Add(showrooms);
                    }
                }
            }
            return new OkObjectResult(Showrooms);
        }

        public IActionResult OnGetLoadReceivingType()
        {
            var receivingType = _receivingTypeRepository.GetAll();
            if (receivingType != null)
            {
                ReceivingTypes = new List<ReceivingType>();
                var items = receivingType.Where(cd => cd.IsDeleted == false).ToList();

                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var receivingTypes = new ReceivingType
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Value = item.Value,
                            NumberShipDay = item.NumberShipDay
                        };
                        ReceivingTypes.Add(receivingTypes);
                    }
                }
            }
            return new OkObjectResult(ReceivingTypes);
        }

        public IActionResult OnPostSaveOrder([FromBody] OrderAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult("Đặt hàng không thành công");
            }

            using (var context = new OnlineStoreDbContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _userManager.GetUserAsync(HttpContext.User).Result;
                        if (user == null)
                        {
                            return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }
                        if (user == null)
                        {
                            return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }

                        var cart = _cartRepository.GetCartByCustomerId(user.Id);
                        if (cart == null)
                        {
                            return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }
                        var newAddress = new Address();
                        if (model.Order.ReceivingTypeId == 3)
                        {
                            newAddress = new Address
                            {
                                CustomerId = user.Id,
                                PhoneNumber = model.Address.PhoneNumber,
                                RecipientName = model.Address.RecipientName,
                                ShowRoomAddressId = model.Address.ShowRoomAddressId,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now
                            };
                            //_addressRepository.Add(newAddress);
                            context.Address.Add(newAddress);
                            context.SaveChanges();
                        }
                        var receivingType = _receivingTypeRepository.Find(model.Order.ReceivingTypeId);
                        var newOrder = new DAL.Data.Entities.Order
                        {
                            AddressId = model.Order.ReceivingTypeId == 3 ? newAddress.Id : model.Order.AddressId,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            //DeliveryDate = model.Order.DeliveryDate,
                            DeliveryDate = DateTime.Now.AddDays(receivingType.NumberShipDay),
                            ShippingFee = model.Order.ShippingFee,
                            //SubTotal = model.Order.SubTotal,
                            SubTotal = cart.CartDetails.Sum(x => x.Item.Price * x.Quantity),
                            OrderDate = DateTime.Now,
                            PaymentType = model.Order.PaymentType,
                            //Total = model.Order.Total,
                            ReceivingTypeId = model.Order.ReceivingTypeId,
                            SaleOff = model.Order.SaleOff,
                            Status = OrderStatus.Pending,
                        };
                        newOrder.Total = newOrder.SubTotal + newOrder.ShippingFee - newOrder.SaleOff;
                        //_unitOfWork.OrderRepository.Add(newOrder);
                        context.Order.Add(newOrder);
                        context.SaveChanges();

                        var items = cart.CartDetails.Where(cd => cd.IsDeleted == false).ToList();

                        if (items == null || items.Count() == 0)
                        {
                            transaction.Rollback();
                            return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }
                        foreach (var itemInCart in items)
                        {
                            var item = _itemRepository.Find(itemInCart.ItemId);
                            if (itemInCart.Quantity > item.Quantity)
                            {
                                transaction.Rollback();
                                return new BadRequestObjectResult("Sản phẩm không còn đủ số lượng cho đơn hàng. Quá trình đặt hàng thất bại.");
                            }
                            var newOrderItem = new OrderItem
                            {
                                OrderId = newOrder.Id,
                                Price = item.Price,
                                ItemId = itemInCart.ItemId,
                                Quantity = itemInCart.Quantity,
                                DateCreated = DateTime.Now,
                                DateModified = DateTime.Now,
                                SaleOff = 0,
                                IsDeleted = false,
                                Amount = item.Price * item.Quantity
                            };
                            //_orderItemRepository.Add(newOrderItem);
                            context.OrderItem.Add(newOrderItem);
                            context.SaveChanges();

                            item.Quantity -= itemInCart.Quantity;
                            //_itemRepository.Update(item);
                            context.Item.Update(item);
                            context.SaveChanges();
                        }
                        //_cartRepository.Delete(cart);
                        cart.IsDeleted = true;
                        context.Cart.Update(cart);
                        context.SaveChanges();

                        var cartDetails = cart.CartDetails;
                        foreach (var cartDetail in cartDetails)
                        {
                            cartDetail.IsDeleted = true;
                            cartDetail.Quantity = 0;
                            context.CartDetail.Update(cartDetail);
                            context.SaveChanges();
                        }
                        //_cartDetailRepository.DeleteRange(_cartDetailRepository.GetSome(cd => cd.CartId == cart.Id));
                        //_unitOfWork.Save();
                        //return new OkObjectResult(Orders);
                        transaction.Commit();
                        return new OkObjectResult(new { orderId = newOrder.Id });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        context.Database.RollbackTransaction();
                        return new BadRequestObjectResult("Đặt hàng không thành công");
                    }
                }
            }
        }
    }
}

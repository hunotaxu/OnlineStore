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
using Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Utilities.Commons;
using OnlineStore.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace OnlineStore.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        private readonly IEmailSender _emailSender;
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
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        protected readonly DbContextOptions<OnlineStoreDbContext> _options;

        public CheckoutModel(IReceivingTypeRepository receivingTypeRepository, IShowRoomAddressRepository showRoomAddressRepository, IAddressRepository addressRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, ICartDetailRepository cartDetailRepository,
            IUserRepository userRepository,
            IDefaultAddressRepository defaultAddressRepository,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IEmailSender emailSender,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            DbContextOptions<OnlineStoreDbContext> options)
        {
            _options = options;
            _emailSender = emailSender;
            _itemRepository = itemRepository;
            _razorViewToStringRenderer = razorViewToStringRenderer;
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
                        if (item.Item.Quantity > 0)
                        {
                            var itemCartViewModel = new ItemCartViewModel
                            {
                                ItemId = item.ItemId,
                                Image = (item.Item.ProductImages.Count() > 0) ?
                                        $"/images/client/ProductImages/{item.Item.ProductImages?.FirstOrDefault()?.Name}" : $"/images/client/ProductImages/no-image.jpg",
                                Price = item.Item.Price,
                                ProductName = item.Item.Name,
                                //Quantity = item.Quantity,
                            };
                            if (item.Item.Quantity > item.Quantity)
                            {
                                itemCartViewModel.Quantity = item.Quantity;
                            }
                            else
                            {
                                itemCartViewModel.Quantity = item.Item.Quantity;
                            }
                            ItemInCarts.Add(itemCartViewModel);
                        }
                    }
                }
            }
            if (TempData.Get<List<ItemCartViewModel>>(CommonConstants.ItemsCheckout) != null)
            {
                TempData.Set<List<ItemCartViewModel>>(CommonConstants.ItemsCheckout, null);
            }

            TempData.Set("ItemsCheckout", ItemInCarts);
            TempData.Keep();
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
                }
            }
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
            if (model.AddressId > 0)
            {
                var existAddress = _addressRepository.Find(model.AddressId);
                existAddress.PhoneNumber = model.PhoneNumber;
                existAddress.RecipientName = model.RecipientName;
                existAddress.Ward = model.Ward;
                existAddress.District = model.District;
                existAddress.Province = model.Province;
                existAddress.Detail = model.Detail;
                existAddress.DateModified = DateTime.Now;
                _addressRepository.Update(existAddress);
                return new OkObjectResult(existAddress);
            }
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
            return new OkObjectResult(newAddress);
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

        public async System.Threading.Tasks.Task<IActionResult> OnPostSaveOrderAsync([FromBody] OrderAddressViewModel model)
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
                            return new BadRequestObjectResult("Tất cả sản phẩm trong giỏ không thể đặt. Vui lòng kiểm tra lại giỏ hàng.");
                        }

                        var cart = _cartRepository.GetCartByCustomerId(user.Id);
                        if (cart == null)
                        {
                            return new BadRequestObjectResult("Tất cả sản phẩm trong giỏ không thể đặt. Vui lòng kiểm tra lại giỏ hàng.");
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
                            context.Address.Add(newAddress);
                            context.SaveChanges();
                        }
                        var receivingType = _receivingTypeRepository.Find(model.Order.ReceivingTypeId);

                        var newOrder = new DAL.Data.Entities.Order
                        {
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            DeliveryDate = DateTime.Now.AddDays(receivingType.NumberShipDay),
                            ShippingFee = model.Order.ShippingFee,
                            SubTotal = cart.CartDetails.Sum(x => x.Item.Price * x.Quantity),
                            OrderDate = DateTime.Now,
                            PaymentType = model.Order.PaymentType,
                            ReceivingTypeId = model.Order.ReceivingTypeId,
                            SaleOff = model.Order.SaleOff,
                            Status = model.Order.Status == OrderStatus.ReadyToDeliver ? OrderStatus.ReadyToDeliver : OrderStatus.Pending,
                        };

                        if (model.Order.PaymentType == PaymentType.CreditDebitCard)
                        {
                            Address addressForOnlinePayment = new Address
                            {
                                CustomerId = user.Id,
                                Detail = "",
                                DateCreated = DateTime.Now,
                                RecipientName = user.Name ?? "",
                                PhoneNumber = user.PhoneNumber ?? ""
                            };
                            context.Address.Add(addressForOnlinePayment);
                            context.SaveChanges();
                            newOrder.AddressId = addressForOnlinePayment.Id;
                        }
                        else
                        {
                            newOrder.AddressId = model.Order.ReceivingTypeId == 3 ? newAddress.Id : model.Order.AddressId;
                        }

                        newOrder.Total = newOrder.SubTotal + newOrder.ShippingFee - newOrder.SaleOff;
                        context.Order.Add(newOrder);
                        context.SaveChanges();


                        var items = cart.CartDetails.Where(cd => cd.IsDeleted == false && cd.Item.Quantity > 0).ToList();

                        if (items == null || items.Count() == 0)
                        {
                            transaction.Rollback();
                            return new BadRequestObjectResult("Tất cả sản phẩm trong giỏ không thể đặt. Vui lòng kiểm tra lại giỏ hàng.");
                        }

                        var itemsCheckout = TempData.Get<List<ItemCartViewModel>>(CommonConstants.ItemsCheckout);

                        //if (itemsCheckout.Sum(x => x.Quantity) != items.Sum(x => x.Item.Quantity))
                        //{
                        //    transaction.Rollback();
                        //    return new BadRequestObjectResult("Các sản phẩm trong giỏ đã có sự thay đổi về số lượng từ hệ thống. Vui lòng kiểm tra lại giỏ hàng.");
                        //}

                        if (itemsCheckout.Sum(x => x.Price) != items.Sum(x => x.Item.Price))
                        {
                            transaction.Rollback();
                            return new BadRequestObjectResult("Các sản phẩm trong giỏ đã có sự thay đổi về giá từ hệ thống. Vui lòng kiểm tra lại giỏ hàng.");
                        }
                        foreach (var itemInCart in items)
                        {
                            if (itemsCheckout.Any(x => x.Quantity > itemInCart.Item.Quantity))
                            {
                                transaction.Rollback();
                                return new BadRequestObjectResult("Các sản phẩm trong giỏ đã có sự thay đổi về số lượng từ hệ thống. Vui lòng kiểm tra lại giỏ hàng.");
                            }

                            var item = _itemRepository.Find(itemInCart.ItemId);
                            if (item.IsDeleted == false)
                            {
                                if (itemInCart.Quantity > item.Quantity)
                                {
                                    transaction.Rollback();
                                    return new BadRequestObjectResult("Sản phẩm không còn đủ số lượng cho đơn hàng. Quá trình đặt hàng thất bại. Vui lòng kiểm tra lại giỏ hàng");
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
                                    Amount = item.Price * itemInCart.Quantity
                                };
                                context.OrderItem.Add(newOrderItem);
                                context.SaveChanges();

                                item.Quantity -= itemInCart.Quantity;
                                context.Item.Update(item);
                                context.SaveChanges();
                            }
                        }
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
                        transaction.Commit();
                        var url = Url.Page("/Order/MyOrder", pageHandler: null, values: new { orderId = newOrder.Id }, protocol: Request.Scheme);
                        var confirmAccountModel = new OrderEmailViewModel
                        {
                            Url = url,
                            LetterDescription = $@"Yêu cầu đặt hàng cho đơn hàng #{newOrder.Id} của bạn đã được tiếp nhận và đang chờ nhà bán hàng xử lý. 
                                                  Thời gian đặt hàng vào lúc {string.Format("{0:HH:mm}", newOrder.OrderDate)} ngày {string.Format("{0:d/M/yyyy}", newOrder.OrderDate)}. 
                                                  Chúng tôi sẽ tiếp tục cập nhật với bạn về trạng thái tiếp theo của đơn hàng."
                        };
                        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("~/Pages/Emails/ConfirmOrderEmail.cshtml", confirmAccountModel);
                        await _emailSender.SendEmailAsync(user.Email, "Xác nhận đơn hàng từ TimiShop", body);
                        return new OkObjectResult(new { orderId = newOrder.Id, email = user.Email });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new BadRequestObjectResult("Đặt hàng không thành công");
                    }
                }
            }
        }
    }
}

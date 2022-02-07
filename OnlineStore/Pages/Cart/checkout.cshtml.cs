using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models.ViewModels.Item;
using OnlineStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Commons;
using Utilities.Extensions;

namespace OnlineStore.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        private readonly ILogger<CheckoutModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDefaultAddressRepository _defaultAddressRepository;
        private readonly ICartRepository _cartRepository;
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

        public CheckoutModel(IReceivingTypeRepository receivingTypeRepository, IShowRoomAddressRepository showRoomAddressRepository, IAddressRepository addressRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, ICartDetailRepository cartDetailRepository,
            IDefaultAddressRepository defaultAddressRepository,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IEmailSender emailSender,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            ILogger<CheckoutModel> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _emailSender = emailSender;
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _defaultAddressRepository = defaultAddressRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _addressRepository = addressRepository;
            _showRoomAddressRepository = showRoomAddressRepository;
            _receivingTypeRepository = receivingTypeRepository;
            _unitOfWork = unitOfWork;
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
                List<CartDetail> items = cart.CartDetails.Where(cd => cd.IsDeleted == false).ToList();
                if (items?.Any() == true)
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
            TempData.Set(CommonConstants.ItemsCheckout, ItemInCarts);
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

        public async Task<IActionResult> OnPostSaveOrderAsync([FromBody] OrderAddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult("Đã xảy ra lỗi. Đặt hàng không thành công");
            }

            try
            {
                ApplicationUser user = _userManager.GetUserAsync(HttpContext.User).Result;
                if (user == null)
                {
                    return new BadRequestObjectResult("Đã xảy ra lỗi. Đặt hàng không thành công");
                }

                DAL.Data.Entities.Cart cart = _unitOfWork.CartRepository.GetCartByCustomerId(user.Id);
                if (cart == null)
                {
                    return new BadRequestObjectResult("Đã xảy ra lỗi. Đặt hàng không thành công");
                }

                List<CartDetail> itemsInCart = cart.CartDetails.Where(cd => !cd.IsDeleted && cd.Item.Quantity > 0).ToList();
                if (itemsInCart?.Any() == false)
                {
                    _unitOfWork.Rollback();
                    return new BadRequestObjectResult("Tất cả sản phẩm trong giỏ không tồn tại. Vui lòng kiểm tra lại giỏ hàng.");
                }
                List<ItemCartViewModel> temporaryItemsCheckout = TempData.Get<List<ItemCartViewModel>>(CommonConstants.ItemsCheckout);
                if (temporaryItemsCheckout.Sum(x => x.Price) != itemsInCart.Sum(x => x.Item.Price))
                {
                    _unitOfWork.Rollback();
                    return new BadRequestObjectResult("Các sản phẩm đã có sự thay đổi về giá từ hệ thống. Vui lòng kiểm tra lại giỏ hàng.");
                }
                foreach (var item in itemsInCart)
                {
                    if (temporaryItemsCheckout.Any(x => x.Quantity > item.Item.Quantity))
                    {
                        _unitOfWork.Rollback();
                        return new BadRequestObjectResult("Sản phẩm đã có sự thay đổi về số lượng từ hệ thống. Đặt hàng không thành công.");
                    }

                    if (!item.Item.IsDeleted)
                    {
                        if (item.Quantity > item.Item.Quantity)
                        {
                            _unitOfWork.Rollback();
                            return new BadRequestObjectResult("Sản phẩm không còn đủ số lượng cho đơn hàng. Quá trình đặt hàng thất bại. Vui lòng kiểm tra lại giỏ hàng");
                        }

                        item.Item.Quantity -= item.Quantity;
                        _unitOfWork.ItemRepository.Update(item.Item);
                        _unitOfWork.Save();
                    }
                }

                cart.IsDeleted = true;
                _unitOfWork.CartRepository.Update(cart);

                Address newAddress = new Address();
                if (model.Order.ReceivingTypeId == (int)ReceivingTypeEnum.NoDelivery)
                {
                    newAddress.CustomerId = user.Id;
                    newAddress.PhoneNumber = model.Address.PhoneNumber;
                    newAddress.RecipientName = model.Address.RecipientName;
                    newAddress.ShowRoomAddressId = model.Address.ShowRoomAddressId;
                    newAddress.DateCreated = DateTime.Now;
                    newAddress.DateModified = DateTime.Now;
                    _unitOfWork.AddressRepository.Add(newAddress);
                    _unitOfWork.Save();
                }
                ReceivingType receivingType = _receivingTypeRepository.Find((int)model.Order.ReceivingTypeId);

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
                    Status = model.Order.Status == OrderStatus.ReadyToDeliver ? OrderStatus.ReadyToDeliver : OrderStatus.Pending
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
                    _unitOfWork.AddressRepository.Add(addressForOnlinePayment);
                    _unitOfWork.Save();
                    newOrder.AddressId = addressForOnlinePayment.Id;
                }
                else
                {
                    newOrder.AddressId = model.Order.ReceivingTypeId == (int)ReceivingTypeEnum.NoDelivery ? newAddress.Id : model.Order.AddressId;
                }

                newOrder.Total = newOrder.SubTotal + newOrder.ShippingFee - newOrder.SaleOff;
                _unitOfWork.OrderRepository.Add(newOrder);
                _unitOfWork.Save();

                itemsInCart.ForEach(item =>
                {
                    _unitOfWork.OrderItemRepository.Add(new OrderItem
                    {
                        OrderId = newOrder.Id,
                        Price = item.Item.Price,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        SaleOff = 0,
                        IsDeleted = false,
                        Amount = item.Item.Price * item.Quantity
                    });
                });
                _unitOfWork.CartDetailRepository.DeleteRange(itemsInCart);
                _unitOfWork.Commit();
                string url = Url.Page("/Order/MyOrder", pageHandler: null, values: new { orderId = newOrder.Id }, protocol: Request.Scheme);
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
                _logger.LogError(ex.Message);
                _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                return new BadRequestObjectResult("Đã có lỗi xảy ra. Đặt hàng không thành công");
            }
        }
    }
}
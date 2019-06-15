using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using OnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OnlineStore.Pages.Order
{
    public class CheckoutModel : PageModel
    {
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        public readonly IWardRepository _wardRepository;
        public readonly IAddressRepository _addressRepository;

        public CheckoutModel(IAddressRepository addressRepository, IWardRepository wardRepository, IDistrictRepository districtRepository, IProvinceRepository provinceRepository, IItemRepository itemRepository, UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository, ICartDetailRepository cartDetailRepository, IUserAddressRepository userAddressRepository
            , IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _cartDetailRepository = cartDetailRepository;
            _cartRepository = cartRepository;
            _userManager = userManager;
            _userAddressRepository = userAddressRepository;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _addressRepository = addressRepository;
        }

        [BindProperty]
        public List<ItemCartViewModel> ItemInCarts { get; set; }
        [BindProperty]
        public List<UserAddressViewModel> UserAddresses { get; set; }
        [BindProperty]
        public List<Province> Provinces { get; set; }
        [BindProperty]
        public List<District> Districts { get; set; }
        [BindProperty]
        public List<Ward> Wards { get; set; }

        public ActionResult OnGet()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (user == null || _userRepository.IsAdmin(user))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = "/Cart/Index" });
            }
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
                        var itemCartViewModel = new ItemCartViewModel
                        {
                            ItemId = item.ItemId,
                            Image = $"/images/client/ProductImages/{item.Item.Image}",
                            Price = item.Item.Price,
                            ProductName = item.Item.Name,
                            Quantity = (item.Quantity < item.Item.Quantity || item.Item.Quantity == 0) ? item.Quantity : item.Item.Quantity,
                            MaxQuantity = item.Item.Quantity
                        };
                        ItemInCarts.Add(itemCartViewModel);
                    }
                }
            }
            return new OkObjectResult(ItemInCarts);
        }
        public IActionResult OnGetLoadAddress()
        {
            var useraddress = _userAddressRepository.GetByUserId(_userManager.GetUserAsync(HttpContext.User).Result.Id);
            if (useraddress != null)
            {
                UserAddresses = new List<UserAddressViewModel>();
                var items = useraddress.Where(cd => cd.IsDeleted == false).ToList();
                if (items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        var userAddress = new UserAddressViewModel
                        {
                            AddressId = item.AddressId,
                            CustomerId = item.CustomerId,
                            PhoneNumber = item.PhoneNumber,
                            RecipientName = item.RecipientName,
                            Province = item.Address.Province,
                            District = item.Address.District,
                            Ward = item.Address.Ward,
                            Detail = item.Address.Detail
                        };
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
            if (user != null && !_userRepository.IsAdmin(user))
            {

                var newAddress = new DAL.Data.Entities.Address
                {
                    Ward = model.Ward,
                    District = model.District,
                    Province = model.Province,
                    Detail = model.Detail,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                };
                _addressRepository.Add(newAddress);
                _userAddressRepository.Add(new UserAddress
                {
                    CustomerId = _userManager.GetUserAsync(HttpContext.User).Result.Id,
                    AddressId = newAddress.Id,
                    PhoneNumber = model.PhoneNumber,
                    RecipientName = model.RecipientName,
                });
            }
            return new OkObjectResult(model);
        }





    }
}
using DAL.Data.Entities;
using DAL.Data.Enums;
using DAL.EF;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models.ViewModels;
using System;
using System.Linq;

namespace OnlineStore.Services
{
    public class OrderService : IOrderService
    {
        protected readonly OnlineStoreDbContext Db;
        protected readonly DbContextOptions<OnlineStoreDbContext> _options;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;
        public readonly IOrderRepository _orderRepository;
        public readonly IAddressRepository _addressRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IReceivingTypeRepository _receivingTypeRepository;
        public readonly IOrderItemRepository _orderItemRepository;

        public OrderService(DbContextOptions<OnlineStoreDbContext> options,
            UserManager<ApplicationUser> userManager,
            ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IAddressRepository addressRepository,
            ICartDetailRepository cartDetailRepository,
            IItemRepository itemRepository,
            IOrderItemRepository orderItemRepository,
            IReceivingTypeRepository receivingTypeRepository)
        {
            //Db = new OnlineStoreDbContext(options);
            _userManager = userManager;
            _receivingTypeRepository = receivingTypeRepository;
            _cartDetailRepository = cartDetailRepository;
            _itemRepository = itemRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _options = options;
        }

        public bool SaveOrder(OrderAddressViewModel model, ApplicationUser user, out string error)
        {
            using (var context = new OnlineStoreDbContext(_options))
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (user == null)
                        {
                            error = "Giỏ hàng không tồn tại.";
                            return false;
                            //return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }

                        var cart = _cartRepository.GetCartByCustomerId(user.Id);
                        if (cart == null)
                        {
                            error = "Giỏ hàng không tồn tại.";
                            return false;
                            //return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }
                        //context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
                        //context.SaveChanges();

                        //context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/visualstudio" });
                        //context.SaveChanges();

                        //var blogs = context.Blogs
                        //    .OrderBy(b => b.Url)
                        //    .ToList();

                        // Commit transaction if all commands succeed, transaction will auto-rollback
                        // when disposed if either commands fails
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
                            _addressRepository.Add(newAddress);
                        }
                        var receivingType = _receivingTypeRepository.Find(model.Order.ReceivingTypeId);
                        var newOrder = new Order
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
                        _orderRepository.Add(newOrder);

                        var items = cart.CartDetails.Where(cd => cd.IsDeleted == false).ToList();
                        if (items == null || items.Count() == 0)
                        {
                            error = "Giỏ hàng không tồn tại.";
                            return false;
                            //return new BadRequestObjectResult("Giỏ hàng không tồn tại.");
                        }
                        foreach (var itemInCart in items)
                        {
                            var item = _itemRepository.Find(itemInCart.ItemId);
                            if (itemInCart.Quantity > item.Quantity)
                            {
                                error = "Sản phẩm không còn đủ số lượng. Quá trình đặt hàng thất bại.";
                                return false;
                                //return new BadRequestObjectResult("Sản phẩm không còn đủ số lượng. Quá trình đặt hàng thất bại.");
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
                            _orderItemRepository.Add(newOrderItem);
                            item.Quantity -= itemInCart.Quantity;
                            _itemRepository.Update(item);
                        }
                        _cartRepository.Delete(cart);
                        _cartDetailRepository.DeleteRange(_cartDetailRepository.GetSome(cd => cd.CartId == cart.Id));
                        //_unitOfWork.Save();
                        //return new OkObjectResult(Orders);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        error = "Đặt hàng không thành công";
                        return false;
                    }
                }
            }
            error = "Đặt hàng thành công";
            return true;
        }
    }
}

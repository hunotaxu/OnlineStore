using AutoMapper;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using Utilities.Commons;

namespace OnlineStore.Areas.Admin.ViewComponents
{
    public class OrderProductInfoViewComponent : ViewComponent
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        [TempData]
        public decimal TotalAmount { get; set; }

        public OrderProductInfoViewComponent(IProductImagesRepository productImagesRepository, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
        {
            _productImagesRepository = productImagesRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            TotalAmount = 0;
        }

        public Task<IViewComponentResult> InvokeAsync(int orderId)
        {
            var order = _orderRepository.Find(orderId);
            var lineItems = _orderItemRepository.GetSome(x => x.OrderId == orderId);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");

            List<OrderProductInfoViewModel> orderProducts = new List<OrderProductInfoViewModel>();
            if (lineItems?.Any() == true)
            {
                decimal total = 0;
                foreach (var item in lineItems)
                {
                    var listImg = _productImagesRepository.GetSome(x => x.ItemId == item.ItemId).ToList();
                    var orderProduct = new OrderProductInfoViewModel
                    {
                        ProductName = item.Item.Name,
                        Amount = CommonFunctions.FormatNumber(item.Amount, 0),
                        Image = listImg?.Any() == true ? listImg[0].Name : string.Empty,
                        Price = CommonFunctions.FormatNumber(item.Item.Price, 0),
                        Quantity = item.Quantity,
                        SaleOff = item.SaleOff ?? 0
                    };
                    total += decimal.Parse(orderProduct.Amount);
                    orderProducts.Add(orderProduct);
                }
                TempData["TotalAmount"] = CommonFunctions.FormatNumber(total, 0);
            }
            return Task.FromResult<IViewComponentResult>(View(orderProducts));
        }
    }
}
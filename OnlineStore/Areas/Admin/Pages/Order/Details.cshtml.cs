using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;
using Utilities;
using OnlineStore.Services;
//using Core;

namespace OnlineStore.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPdfService _pdfService;
        //private readonly OrderSettings _orderSettings;

        //public DetailsModel(IOrderRepository orderRepository, IPdfService pdfService, OrderSettings orderSettings)
        public DetailsModel(IOrderRepository orderRepository, IPdfService pdfService)
        {
            _orderRepository = orderRepository;
            _pdfService = pdfService;
            //_orderSettings = orderSettings;
        }

        public DAL.Data.Entities.Order Order { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = _orderRepository.Find(x => x.Id == id);

            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPostSaveEntity([FromBody] DAL.Data.Entities.Order model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (model.Id == 0)
            {
                return new BadRequestResult();
            }

            var order = _orderRepository.Find(model.Id);
            if (model.Status != 0)
            {
                order.Status = model.Status;
            }
            if (model.DeliveryDate.HasValue)
            {
                if (DateTime.Compare(model.DeliveryDate.Value, order.OrderDate) < 0)
                {
                    return new BadRequestObjectResult("Không thể đặt ngày giao trước ngày đặt hàng");
                }
                order.DeliveryDate = model.DeliveryDate.Value;
            }
            order.DateModified = DateTime.Now;
            _orderRepository.Update(order);

            return new OkObjectResult(order);
        }

        public virtual IActionResult OnGetPdfInvoice(int orderId)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageOrders)) { 
            //    return AccessDeniedView();
            //}

            //a vendor should have access only to his products
            //var vendorId = 0;
            //if (_workContext.CurrentVendor != null)
            //{
            //    vendorId = _workContext.CurrentVendor.Id;
            //}

            //var order = _orderService.GetOrderById(orderId);
            var order = _orderRepository.Find(orderId);
            var orders = new List<DAL.Data.Entities.Order>
            {
                order
            };

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                //_pdfService.PrintOrdersToPdf(stream, orders, _orderSettings.GeneratePdfInvoiceInCustomerLanguage ? 0 : _workContext.WorkingLanguage.Id, vendorId);
                _pdfService.PrintOrdersToPdf(stream, orders);
                bytes = stream.ToArray();
            }

            return File(bytes, MimeTypes.ApplicationPdf, $"order_{order.Id}_{DateTime.Now.ToString("dd/MM/yyyy")}.pdf");
        }
    }
}
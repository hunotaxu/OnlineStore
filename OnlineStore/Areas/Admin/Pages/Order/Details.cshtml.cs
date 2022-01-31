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
using Microsoft.Extensions.Logging;
using OnlineStore.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace OnlineStore.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPdfService _pdfService;
        private readonly ILogger<DetailsModel> _logger;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly IEmailSender _emailSender;

        public DetailsModel(IOrderRepository orderRepository,
            IPdfService pdfService,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            ILogger<DetailsModel> logger,
            IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _pdfService = pdfService;
            _emailSender = emailSender;
            _logger = logger;
            _razorViewToStringRenderer = razorViewToStringRenderer;
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

        public async System.Threading.Tasks.Task<IActionResult> OnPostSaveEntityAsync([FromBody] DAL.Data.Entities.Order model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                    return new BadRequestObjectResult("Đã xảy ra lỗi");
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
                var cus = order.Address?.Customer;
                if (!string.IsNullOrEmpty(cus?.Email))
                {
                    var url = Url.Page("/Order/MyOrder", pageHandler: null, values: new { orderId = order.Id }, protocol: Request.Scheme);
                    var confirmAccountModel = new OrderEmailViewModel
                    {
                        Url = url
                    };

                    if (model.Status == DAL.Data.Enums.OrderStatus.Shipped)
                    {
                        confirmAccountModel.LetterDescription = $@"Đơn hàng #{order.Id} đang trên đường vận chuyển. Thời gian giao hàng dự kiến vào ngày {string.Format("{0:d/M/yyyy}", order.DeliveryDate)}. 
                                                               Chúng tôi sẽ tiếp tục cập nhật với bạn về trạng thái tiếp theo của đơn hàng.";
                        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("~/Pages/Emails/ConfirmOrderEmail.cshtml", confirmAccountModel);
                        await _emailSender.SendEmailAsync(cus.Email, $@"Đơn hàng #{order.Id} đang trên đường vận chuyển.", body);
                    }
                    else if (model.Status == DAL.Data.Enums.OrderStatus.Delivered)
                    {
                        confirmAccountModel.LetterDescription = $@"Đơn hàng #{order.Id} của bạn đã được giao đầy đủ với các sản phẩm được liệt kê ở chi tiết đơn hàng bên dưới. TimiShop hi vọng bạn hài lòng với các sản phẩm này!";
                        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("~/Pages/Emails/ConfirmOrderEmail.cshtml", confirmAccountModel);
                        await _emailSender.SendEmailAsync(cus.Email, $@"Đơn hàng #{order.Id} đã được giao thành công.", body);
                    }
                    else if (model.Status == DAL.Data.Enums.OrderStatus.Canceled)
                    {
                        confirmAccountModel.LetterDescription = $@"Rất tiếc, đơn hàng #{order.Id} đã được hoàn về kho TimiShop và được tự động hủy trên hệ thống do đối tác giao nhận của TimiShop không liên lạc được với bạn trong quá trình giao hàng.";
                        string body = await _razorViewToStringRenderer.RenderViewToStringAsync("~/Pages/Emails/ConfirmOrderEmail.cshtml", confirmAccountModel);
                        await _emailSender.SendEmailAsync(cus.Email, $@"Đơn hàng #{order.Id} đã giao không thành công.", body);
                    }
                }

                return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when send email", ex);
                return new BadRequestObjectResult("Đã xãy ra lỗi");
            }

        }

        public FileResult OnGetPdfInvoice(int orderId)
        {
            var order = _orderRepository.Find(orderId);

            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintOrdersToPdf(stream, order);
                bytes = stream.ToArray();
            }

            return File(bytes, MimeTypes.ApplicationPdf, $"DonHang_{order.Id}_Ngay_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}
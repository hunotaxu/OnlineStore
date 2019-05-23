using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Collections.Generic;
using System;

namespace OnlineStore.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public DetailsModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
                order.DeliveryDate = model.DeliveryDate.Value;
            }
            order.DateModified = DateTime.Now;
            _orderRepository.Update(order);

            return new OkObjectResult(order);
        }
    }
}
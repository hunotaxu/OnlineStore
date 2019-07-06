using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineStore.Models.ViewModels.Item;
using Utilities.DTOs;

namespace OnlineStore.Areas.Admin.Pages.Order
{
    public class ReceivingTypeManagementModel : PageModel
    {
        private readonly IReceivingTypeRepository _receivingTypeRepository;
        private readonly IOrderRepository _orderRepository;
        public ReceivingTypeManagementModel(
            IReceivingTypeRepository receivingTypeRepository,
            IOrderRepository orderRepository)
        {
            _receivingTypeRepository = receivingTypeRepository;
            _orderRepository = orderRepository;
        }
        public void OnGet()
        {

        }

        //public IActionResult OnGetAllPaging(int? categoryId, string keyword, int pageIndex, int pageSize)
        public IActionResult OnGetAllPaging()
        {
            var model = _receivingTypeRepository.GetAll();
            //var itemsPagination = _mapperConfiguration.CreateMapper().Map<PagedResult<ItemViewModel>>(model);
            return new OkObjectResult(model);
        }

        public IActionResult OnGetById(int id)
        {
            var model = _receivingTypeRepository.Find(id);
            return new OkObjectResult(model);
        }

        public IActionResult OnPostSaveEntity([FromBody] ReceivingType model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (model.Id == 0)
            {
                model.DateCreated = DateTime.Now;
                _receivingTypeRepository.Add(model);
            }
            else
            {
                var receivingType = _receivingTypeRepository.Find(model.Id);
                receivingType.Name = model.Name;
                receivingType.Value = model.Value;
                receivingType.NumberShipDay = model.NumberShipDay;
                receivingType.DateModified = DateTime.Now;
                //item.Description = model.Description;
                //item.Price = model.Price;
                //item.OriginalPrice = model.OriginalPrice;
                //item.DateModified = DateTime.Now;
                _receivingTypeRepository.Update(receivingType);
            }
            //var listImages = _productImagesRepository.GetSome(x => x.ItemId == model.Id);
            //_productImagesRepository.DeleteRange(listImages);
            //var listAttachment = TempData.Get<List<ProductImages>>(CommonConstants.Attachments);
            //if (listAttachment != null && listAttachment.Count > 0)
            //{
            //    foreach (var attachment in listAttachment)
            //    {
            //        string fileName = SaveAttachment(attachment);

            //        if (!string.IsNullOrEmpty(fileName))
            //        {
            //            _productImagesRepository.Add(new ProductImages()
            //            {
            //                ItemId = model.Id,
            //                Name = fileName,
            //                Path = attachment.Path,
            //                Contents = attachment.Contents
            //            });
            //        }
            //    }
            //}

            return new OkObjectResult(model);
        }

        public IActionResult OnPostDelete([FromBody] ReceivingType model)
        {
            var receivingType = _receivingTypeRepository.Find(model.Id);
            _receivingTypeRepository.Delete(receivingType);
            var orders = _orderRepository.GetSome(x => x.ReceivingTypeId == model.Id && x.IsDeleted == false);
            _orderRepository.DeleteRange(orders);
            return new OkResult();
        }
    }
}
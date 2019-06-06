using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using AutoMapper;
using DAL.Data.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Utilities.Extensions;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models.ViewModels.Item;
using Utilities.Commons;
using Utilities.DTOs;

namespace OnlineStore.Areas.Admin.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IItemRepository _itemRepository;
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IGoodsReceiptDetailRepository _goodsReceiptDetailRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHostingEnvironment hostingEnvironment, ILogger<IndexModel> logger,
                          IItemRepository itemRepository,
                          ICategoryRepository categoryRepository, IProductImagesRepository productImagesRepository,
                          ICommentRepository commentRepository,
                          IGoodsReceiptDetailRepository goodsReceiptDetailRepository,
                          ICartDetailRepository cartDetailRepository,
                          IOrderItemRepository orderItemRepository)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _itemRepository = itemRepository;
            _cartDetailRepository = cartDetailRepository;
            _orderItemRepository = orderItemRepository;
            _commentRepository = commentRepository;
            _goodsReceiptDetailRepository = goodsReceiptDetailRepository;
            _categoryRepository = categoryRepository;
            _productImagesRepository = productImagesRepository;
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Item, ItemViewModel>();
                _ = cfg.CreateMap<DAL.Data.Entities.Category, CategoryViewModel>();
            });
        }

        public IEnumerable<ItemViewModel> Items { get; set; }

        public void OnGet()
        {

        }

        public ActionResult OnGetLoadAttachments(int productId)
        {
            TempData.Remove(CommonConstants.Attachments);
            var attachmentsList = _productImagesRepository.GetSome(x => x.ItemId == productId && x.IsDeleted == false);
            if (attachmentsList != null)
            {
                List<ProductImages> list = attachmentsList.ToList();
                TempData.Set(CommonConstants.Attachments, list);
                TempData.Keep();
            }
            return new JsonResult(new { attachmentsList = attachmentsList });
        }

        public IActionResult OnGetAll()
        {
            var model = _itemRepository.GetAll(i => i.Category);
            Items = _mapperConfiguration.CreateMapper().Map<IEnumerable<ItemViewModel>>(model);
            return new OkObjectResult(Items);
        }

        public IActionResult OnGetById(int id)
        {
            var model = _mapperConfiguration.CreateMapper().Map<ItemViewModel>(_itemRepository.Find(id));
            return new OkObjectResult(model);
        }

        public IActionResult OnGetAllCategories()
        {
            var categories = _mapperConfiguration.CreateMapper()
                .Map<IEnumerable<CategoryViewModel>>(_categoryRepository.GetAll());
            return new OkObjectResult(categories);
        }

        public IActionResult OnGetAllPaging(int? categoryId, string keyword, int pageIndex, int pageSize)
        {
            //var admin = HttpContext.Session.Get<ApplicationUser>(CommonConstants.UserSession);
            //if (admin == null || !_userRepository.IsProductManager(admin.UserName))
            //{
            //    return new JsonResult(new { authenticate = false });
            //}
            var model = _itemRepository.GetAllPaging(categoryId, keyword, pageIndex, pageSize);
            var itemsPagination = _mapperConfiguration.CreateMapper().Map<PagedResult<ItemViewModel>>(model);
            return new OkObjectResult(itemsPagination);
        }

        public IActionResult OnPostSaveEntity([FromBody] Item model)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            if (model.Id == 0)
            {
                model.DateCreated = DateTime.Now;
                _itemRepository.Add(model);
            }
            else
            {
                var item = _itemRepository.Find(model.Id);
                item.Name = model.Name;
                item.BrandName = model.BrandName;
                item.Quantity = model.Quantity;
                item.CategoryId = model.CategoryId;
                item.Description = model.Description;
                item.Price = model.Price;
                item.PromotionPrice = model.PromotionPrice;
                item.DateModified = DateTime.Now;
                _itemRepository.Update(item);
            }
            var listImages = _productImagesRepository.GetSome(x => x.ItemId == model.Id);
            _productImagesRepository.DeleteRange(listImages);
            var listAttachment = TempData.Get<List<ProductImages>>(CommonConstants.Attachments);
            if (listAttachment != null && listAttachment.Count > 0)
            {
                foreach (var attachment in listAttachment)
                {
                    string fileName = SaveAttachment(attachment);

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        _productImagesRepository.Add(new ProductImages()
                        {
                            ItemId = model.Id,
                            Name = fileName,
                            Path = attachment.Path,
                            Contents = attachment.Contents
                        });
                    }
                }
            }

            return new OkObjectResult(model);
        }

        public IActionResult OnPostDelete([FromBody] ItemViewModel model)
        {
            var item = _itemRepository.Find(model.Id);
            _itemRepository.Delete(item);
            var productImages = _productImagesRepository.GetSome(x => x.ItemId == model.Id && x.IsDeleted == false);
            _productImagesRepository.DeleteRange(productImages);
            var comments = _commentRepository.GetSome(x => x.ItemId == model.Id && x.IsDeleted == false);
            _commentRepository.DeleteRange(comments);
            var cartDetails = _cartDetailRepository.GetSome(x => x.ItemId == model.Id && x.IsDeleted == false);
            _cartDetailRepository.DeleteRange(cartDetails);
            var lineItems = _orderItemRepository.GetSome(x => x.ItemId == model.Id && x.IsDeleted == false);
            _orderItemRepository.DeleteRange(lineItems);
            var goodsReceiptDetails = _goodsReceiptDetailRepository.GetSome(x => x.ItemId == model.Id && x.IsDeleted == false);
            _goodsReceiptDetailRepository.DeleteRange(goodsReceiptDetails);
            return new OkResult();
        }

        public string SaveAttachment(ProductImages data)
        {
            try
            {
                var imageFolder = $@"\images\client\ProductImages\";
                var dir = _hostingEnvironment.WebRootPath + imageFolder;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                System.IO.File.WriteAllBytes(Path.Combine(dir, data.Name), data.Contents);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot save attachment", ex.Message);
                data.Name = string.Empty;
            }

            return data.Name;
        }

        public IActionResult OnPostImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, fileName);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs); // Copy the contents of the uploaded file to the FileStream object
                    fs.Flush(); // clears buffer
                }
                _itemRepository.ImportExcel(filePath, categoryId);
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        public IActionResult OnPostExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"SanPham_{DateTime.Now:yyyyMMddhhmmss}.xlxs";

            // http://localhost:4800/account/login Thì http là scheme, localhost là Host, còn /account/login là path
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(directory, sFileName));
            }
            var products = _itemRepository.GetAll();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Danh sách sản phẩm");
                worksheet.Cells["A1"].LoadFromCollection(products, true, OfficeOpenXml.Table.TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save();
            }
            return new OkObjectResult(fileUrl);
        }
    }
}
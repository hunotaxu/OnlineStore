using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using TeduCoreApp.Areas.Admin.Controllers;
using Utilities.Commons;
using Utilities.Extensions;
using DAL.Data.Entities;

namespace OnlineStore.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public async Task UploadImageForCKEditor(IList<IFormFile> upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            DateTime now = DateTime.Now;
            if (upload.Count == 0)
            {
                await HttpContext.Response.WriteAsync("Yêu cầu nhập ảnh");
            }
            else
            {
                var file = upload[0];
                var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                await HttpContext.Response.WriteAsync("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", '" + Path.Combine(imageFolder, filename).Replace(@"\", @"/") + "');</script>");
            }
        }
        /// <summary>
        /// Upload image for form
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public IActionResult UploadImage()
        //{
        //    DateTime now = DateTime.Now;
        //    var files = Request.Form.Files;
        //    if (files.Count == 0)
        //    {
        //        return new BadRequestObjectResult(files);
        //    }
        //    else
        //    {
        //        var file = files[0];
        //        var filename = ContentDispositionHeaderValue
        //                            .Parse(file.ContentDisposition)
        //                            .FileName
        //                            .Trim('"');

        //        var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

        //        string folder = _hostingEnvironment.WebRootPath + imageFolder;

        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        string filePath = Path.Combine(folder, filename);
        //        using (FileStream fs = System.IO.File.Create(filePath))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }
        //        return new OkObjectResult(Path.Combine(imageFolder, filename).Replace(@"\", @"/"));
        //    }
        //}

        public ActionResult TemporaryStoreAttachment(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (TempData.Get<List<ProductImages>>(CommonConstants.Attachments) == null)
                {
                    TempData.Set(CommonConstants.Attachments, new List<ProductImages>());
                }
                var listAttachment = TempData.Get<List<ProductImages>>(CommonConstants.Attachments);
                TempData.Keep();
                if (listAttachment.Count >= 5)
                {
                    return Json("maxfilesexceeded");
                }
                if (listAttachment.Exists(x => x.Name == file.FileName))
                {
                    return Json("duplicated");
                }
                BinaryReader b = new BinaryReader(file.OpenReadStream());
                byte[] binData = b.ReadBytes((int)file.Length);
                listAttachment.Add(new ProductImages
                {
                    Name = file.FileName,
                    ContentType = file.ContentType,
                    Contents = binData
                });
                TempData.Set(CommonConstants.Attachments, listAttachment);
                TempData.Keep();
            }

            return Json("success");
        }
        public ActionResult TemporaryRemoveAllAttachment()
        {
            TempData.Remove(CommonConstants.Attachments);
            TempData.Keep();
            return Json("success");
        }

        public ActionResult TemporaryRemoveAttachment(string fileName)
        {
            var listAttachment = TempData.Get<List<ProductImages>>(CommonConstants.Attachments) as List<ProductImages>;
            if (listAttachment == null)
            {
                return Json("success");
            }

            listAttachment.Remove(listAttachment.Find(x => x.Name == fileName));
            TempData.Set(CommonConstants.Attachments, listAttachment);
            TempData.Keep();
            return Json("success");
        }
    }
}
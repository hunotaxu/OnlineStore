using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using TeduCoreApp.Areas.Admin.Controllers;
using TTL.Solution.Areas.Exams.Models;

namespace OnlineStore.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        private const string ATTACHMENTS = "attachments";
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
                var listAttachment = TempData.Peek(ATTACHMENTS) != null ? TempData.Peek(ATTACHMENTS) as List<AttachmentModel> : new List<AttachmentModel>();

                if (listAttachment.Exists(x => x.Name == file.FileName))
                {
                    return Json("duplicated");
                }

                BinaryReader b = new BinaryReader(file.OpenReadStream());
                byte[] binData = b.ReadBytes((int)file.Length);
                listAttachment.Add(new AttachmentModel
                {
                    Name = file.FileName,
                    ContentType = file.ContentType,
                    Contents = binData
                });
            }

            return Json("success");
        }

        public ActionResult TemporaryRemoveAttachment(string fileName)
        {
            var listAttachment = TempData.Peek(ATTACHMENTS) as List<AttachmentModel>;

            if (listAttachment == null)
            {
                return Json("success");
            }

            listAttachment.Remove(listAttachment.Find(x => x.Name == fileName));
            return Json("success");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IViewRenderService _viewRenderService;

        public HomeController()
        {
        }

        public HomeController(IRazorViewEngine razorViewEngine, IHttpContextAccessor httpContextAccessor, IViewRenderService viewRenderService)
        {
            _razorViewEngine = razorViewEngine;
            _httpContextAccessor = httpContextAccessor;
            _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var res = await _viewRenderService.RenderToStringAsync("View1", new Info(1, "one"));

            return View();
        }

        [HttpPost]
        public ActionResult UploadFile()//(IEnumerable<IFormFile> files)
        {
            var files = Request?.Form?.Files;

            long size = files.Sum(f => f.Length);

            var filePath = @"C:\fastclick\f1.txt";

            foreach (var formFile in files)
            {
                var filetype = Path.GetExtension(formFile.FileName).Replace('.', ' ').Trim();

                using (BinaryReader reader = new BinaryReader(formFile.OpenReadStream()))
                {
                    var tempFileBytes = reader.ReadBytes((int)formFile.Length);

                    IsValidFile(tempFileBytes, FileType.Image, filetype);
                }


                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }

            return View();
        }

        private enum ImageFileExtension
        {
            none = 0,
            jpg = 1,
            jpeg = 2,
            bmp = 3,
            gif = 4,
            png = 5
        }
        public enum FileType
        {
            Image = 1,
            Video = 2,
            PDF = 3,
            Text = 4,
            DOC = 5,
            DOCX = 6,
            PPT = 7,
        }

        public static bool IsValidFile(byte[] bytFile, FileType flType, string fileContentType)
        {
            bool isvalid = false;

            if (flType == FileType.Image)
            {
                isvalid = IsValidImageFile(bytFile, fileContentType);
            }

            return isvalid;
        }

        public static bool IsValidImageFile(byte[] bytFile, string fileContentType)
        {
            bool isvalid = false;

            byte[] chkBytejpg = { 255, 216, 255, 224 };
            byte[] chkBytebmp = { 66, 77 };
            byte[] chkBytegif = { 71, 73, 70, 56 };
            byte[] chkBytepng = { 137, 80, 78, 71 };


            ImageFileExtension imgfileExtn = ImageFileExtension.none;

            if (fileContentType.Contains("jpg") | fileContentType.Contains("jpeg"))
            {
                imgfileExtn = ImageFileExtension.jpg;
            }
            else if (fileContentType.Contains("png"))
            {
                imgfileExtn = ImageFileExtension.png;
            }
            else if (fileContentType.Contains("bmp"))
            {
                imgfileExtn = ImageFileExtension.bmp;
            }
            else if (fileContentType.Contains("gif"))
            {
                imgfileExtn = ImageFileExtension.gif;
            }

            if (imgfileExtn == ImageFileExtension.jpg || imgfileExtn == ImageFileExtension.jpeg)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (int i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytejpg[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }


            if (imgfileExtn == ImageFileExtension.png)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (int i = 0; i <= 3; i++)
                    {
                        if (bytFile[i] == chkBytepng[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }


            if (imgfileExtn == ImageFileExtension.bmp)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (int i = 0; i <= 1; i++)
                    {
                        if (bytFile[i] == chkBytebmp[i])
                        {
                            j = j + 1;
                            if (j == 2)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            if (imgfileExtn == ImageFileExtension.gif)
            {
                if (bytFile.Length >= 4)
                {
                    int j = 0;
                    for (int i = 0; i <= 1; i++)
                    {
                        if (bytFile[i] == chkBytegif[i])
                        {
                            j = j + 1;
                            if (j == 3)
                            {
                                isvalid = true;
                            }
                        }
                    }
                }
            }

            return isvalid;
        }

        private Maybe<Info> Find()
        {
            //return Maybe<Info>.OfValue(new Info(1, "one"));
            return Maybe<Info>.NoValue();
        }
    }

    public struct Maybe<T>
    {
        private Maybe(bool hasValue, T value)
        {
            HasValue = hasValue;
            Value = value;
        }

        public bool HasValue { get; }

        public T Value { get; }

        public static Maybe<T> OfValue(T value)
        {
            if (value == null)
                throw new Exception("Value cannot be null");

            return new Maybe<T>(true, value);
        }

        public static Maybe<T> NoValue()
        {
            return new Maybe<T>(false, default);
        }
    }

}

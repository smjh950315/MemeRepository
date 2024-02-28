using Cyh;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Lib.Interface;
using MemeRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    public class ImageController : MyControllerBase
    {
        IImageManager _ImageManager;
        public ImageController(
            IWebAppConfigurations webAppConfigurations,
            IImageManager imageManager)
            : base(webAppConfigurations) {
            this._ImageManager = imageManager;
        }

        public IActionResult Index() {
            return this.View();
        }

        public IActionResult Details(long? id) {
            if (id == null) return this.NotFound();

            return this.View();
        }

        public IActionResult Upload() {
            return this.View();
        }

        [HttpPost]
        public IActionResult Upload(ImageUploadViewModel? imageUpload) {
            if (imageUpload == null) { return this.View(); }
            if (imageUpload.Name.IsNullOrEmpty() || imageUpload.ImageFile == null) { return this.View(); }

            byte[] imgData;

            using (Stream str = imageUpload.ImageFile.OpenReadStream()) {
                imgData = new byte[str.Length];
                str.Read(imgData, 0, imgData.Length);
            }

            //this._ImageManager.Save(new ImageDetailViewModel
            //{
            //    NAME = imageUpload.Name,
            //    DATA = imgData,
            //    CREATED = DateTime.Now,
            //    UPDATED = DateTime.Now,
            //    TYPE = imageUpload.ImageFile.ContentType,
            //    SIZE = (int)imageUpload.ImageFile.Length
            //}, null, true);

            return this.View();
        }
    }
}

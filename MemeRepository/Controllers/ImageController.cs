using Cyh;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.ViewModels;
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
        //Expression<Func<ImageInformationViewModel, IMAGE>> IViewModelHelper<IMAGE, ImageInformationViewModel>.GetExprToDataModel() {
        //    return x => new IMAGE
        //    {
        //        ID = x.Id,
        //        NAME = x.Name,
        //        DATA = x.Data,
        //        SIZE = x.Size,
        //        TYPE = x.Type,
        //        CREATED = x.Created,
        //        UPDATED = DateTime.Now,
        //    };
        //}

        //Expression<Func<IMAGE, ImageInformationViewModel>> IViewModelHelper<IMAGE, ImageInformationViewModel>.GetExprToViewModel() {
        //    return x => new ImageInformationViewModel
        //    {
        //        Id = x.ID,
        //        Name = x.NAME,
        //        Data = x.DATA,
        //        Size = x.SIZE,
        //        Type = x.TYPE,
        //        Created = x.CREATED,
        //        Updated = x.UPDATED,
        //    };
        //}

        //public Expression<Func<TAG, TagInformationViewModel>> GetExprToViewModel() {
        //    return x => new TagInformationViewModel
        //    {
        //        TagId = x.ID,
        //        TagName = x.NAME,
        //        Created = x.CREATED,
        //        Updated = x.UPDATED,
        //    };
        //}

        //public Expression<Func<TagInformationViewModel, TAG>> GetExprToDataModel() {
        //    return x => new TAG
        //    {
        //        ID = x.TagId,
        //        NAME = x.TagName,
        //        CREATED = x.Created,
        //        UPDATED = x.Updated
        //    };
        //}

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

            this._ImageManager.Save(new ImageViewModel
            {
                NAME = imageUpload.Name,
                DATA = imgData,
                CREATED = DateTime.Now,
                UPDATED = DateTime.Now,
                TYPE = imageUpload.ImageFile.ContentType,
                SIZE = (int)imageUpload.ImageFile.Length
            }, null, true);

            return this.View();
        }
    }
}

using Cyh;
using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.Modules.ModViewData;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Db.Models;
using MemeRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace MemeRepository.Controllers
{
    public class ImageController : MyModelAccessController, IViewModelHelper<IMAGE, ImageInformation>, IViewModelHelper<TAG, TagInformation>
    {
        public ImageController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreater)
            : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {

        }

        public IDataManager<IMAGE>? DefaultDataManager { get; set; }
        IDataManager<TAG>? IModelHelper<TAG>.DefaultDataManager { get; set; }

        Expression<Func<ImageInformation, IMAGE>> IViewModelHelper<IMAGE, ImageInformation>.GetExprToDataModel() {
            return x => new IMAGE
            {
                ID = x.Id,
                NAME = x.Name,
                DATA = x.Data,
                SIZE = x.Size,
                TYPE = x.Type,
                CREATED = x.Created,
                UPDATED = DateTime.Now,
            };
        }

        Expression<Func<IMAGE, ImageInformation>> IViewModelHelper<IMAGE, ImageInformation>.GetExprToViewModel() {
            return x => new ImageInformation
            {
                Id = x.ID,
                Name = x.NAME,
                Data = x.DATA,
                Size = x.SIZE,
                Type = x.TYPE,
                Created = x.CREATED,
                Updated = x.UPDATED,
            };
        }

        public Expression<Func<TAG, TagInformation>> GetExprToViewModel() {
            return x => new TagInformation
            {
                TagId = x.ID,
                TagName = x.NAME,
                Created = x.CREATED,
                Updated = x.UPDATED,
            };
        }

        public Expression<Func<TagInformation, TAG>> GetExprToDataModel() {
            return x => new TAG
            {
                ID = x.TagId,
                NAME = x.TagName,
                CREATED = x.Created,
                UPDATED = x.Updated
            };
        }

        public IActionResult Index() {
            return this.View();
        }

        public IActionResult Details(long? id) {
            if (id == null) return this.NotFound();
            ImageInformation? imgInfo = this.GetViewModel<IMAGE, ImageInformation>(x => x.ID == id, null);
            if (imgInfo == null) {
                return this.NotFound();
            }
            var tagBindings = this.GetDataManager<TAG_BINDING>().GetDatas(x => x.IMAGE_ID == imgInfo.Id, null);
            List<TagInformation> tags = new List<TagInformation>();
            foreach (var tagBinding in tagBindings) {
                var tagData = this.GetViewModel<TAG, TagInformation>(x => x.ID == tagBinding.ID, null);
                if (tagData != null) {
                    tags.Add(tagData);
                }
            }
            imgInfo.Tags = tags;
            return this.View(imgInfo);
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

            this.SaveDataModel(new IMAGE
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

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
    [ApiController]
    [Route("api/image")]
    public class ApiImageController : MyModelAccessController, IViewModelHelper<IMAGE, ImageInformation>, IModelHelper<TAG>
    {
        public ApiImageController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreater)
            : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {

        }

        public IDataManager<IMAGE>? DefaultDataManager { get; set; }

        IDataManager<TAG>? IModelHelper<TAG>.DefaultDataManager { get; set; }

        Expression<Func<ImageInformation, IMAGE>> IViewModelHelper<IMAGE, ImageInformation>.GetExprToDataModel() {
            Stream stream = null;
            MemoryStream memoryStream = null;
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
                Updated = DateTime.Now,
            };
        }

        [Route("get/single/{id}")]
        [HttpGet]
        public ImageInformation? GetImageViewModel(long id) {
            ImageInformation? imgInfo = this.GetViewModel(x => x.ID == id, null);
            if (imgInfo == null) { return null; }
            TAG_BINDING? tagBinding = this.GetDataManager<TAG_BINDING>().GetData(x => x.IMAGE_ID == id, null);
            if (tagBinding == null) { return imgInfo; }
            IEnumerable<TagInformation> tags = this.GetDataManager<TAG>().GetDatasAs(x => new TagInformation
            {
                TagId = x.ID,
                TagName = x.NAME,
                Created = x.CREATED,
                Updated = x.UPDATED,
            }, x => x.ID == tagBinding.TAG_ID, null);
            imgInfo.Tags = tags;
            return imgInfo;
        }

        [Route("save/single")]
        [HttpPost]
        public IDataTransResult SaveViewModel(IEnumerable<IMAGE> imageViewModel) {
            return this.SaveDataModels(imageViewModel, null, true);
        }

    }
}

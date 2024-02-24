using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Lib.Interface;
using Microsoft.AspNetCore.Mvc;
using ImgViewModel = MemeRepository.Lib.ViewModels.ImageViewModel;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ApiImageController : MyModelAccessController
    {
        IImageManager _ImageManager;
        public ApiImageController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreater,
            IImageManager imageManager)
            : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {
            this._ImageManager = imageManager;
        }

        [Route("get/single/{id}")]
        [HttpGet]
        public ImgViewModel? GetImageViewModel(long id) {
            return this._ImageManager.GetById(id);
        }

        [Route("save/single")]
        [HttpPost]
        public IDataTransResult SaveViewModel(IEnumerable<ImgViewModel> imageViewModel) {
            return this._ImageManager.Save(imageViewModel, null, true);
        }

    }
}

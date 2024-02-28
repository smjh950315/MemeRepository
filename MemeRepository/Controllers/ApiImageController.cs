using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ApiImageController : MyControllerBase
    {
        IImageManager _ImageManager;
        public ApiImageController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreater,
            IImageManager imageManager)
            : base(webAppConfigurations) {
            this._ImageManager = imageManager;
        }

        [Route("get/{id}")]
        [HttpGet]
        public ImageDetailViewModel? GetImageViewModel(long id) {
            return this._ImageManager.GetById(id);
        }

        [Route("save/")]
        [HttpPost]
        public IDataTransResult SaveViewModel(IEnumerable<ImageDetailViewModel> imageViewModel) {
            return this._ImageManager.Save(imageViewModel, null, true);
        }

    }
}

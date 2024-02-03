using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Db.Interface;
using MemeRepository.Db.Models;
using MemeRepository.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : MyCastableModelController<IMAGE, ImageViewModel, IMemeImage>
    {
        public TestController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManager<IMAGE> dataManager)
            : base(webAppConfigurations, dataManagerActivator, dataManager) {

        }

        [Route("get/single")]
        [HttpGet]
        public ImageViewModel? GetImageViewModel(long id) {
            return this.GetViewModel(x => x.ID == id);
        }

        [Route("save/single")]
        [HttpPost]
        public IDataTransResult SaveViewModel(ImageViewModel imageViewModel) {
            return this.SaveFromViewModels(new ImageViewModel[] { imageViewModel }, true);
        }

    }
}

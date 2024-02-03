using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Db.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ApiImageController : MyCastableModelController<Image>
    {
        public ApiImageController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManager<Image> dataManager)
            : base(webAppConfigurations, dataManagerActivator, dataManager) {

        }

        [Route("get/single")]
        [HttpGet]
        public Image? GetImageViewModel(long id) {
            return this.GetDataModel(x => x.ImageID == id);
        }

        [Route("save/single")]
        [HttpPost]
        public IDataTransResult SaveViewModel(IEnumerable<Image> imageViewModel) {
            return this.SaveDataModels(imageViewModel, true);
        }

    }
}

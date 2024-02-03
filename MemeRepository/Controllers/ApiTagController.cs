using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.EFCore;
using Cyh.EFCore.Interface;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Db.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class ApiTagController : MyCastableModelController<Tag>
    {
        public ApiTagController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManager<Tag> dataManager
            ) : base(webAppConfigurations, dataManagerActivator, dataManager) {
        }

        [Route("get_ids")]
        [HttpGet]
        public IEnumerable<long> GetTagIds() {
            return this.GetDataModelsAs(x => x.TagID);
        }

        [Route("get_names")]
        [HttpGet]
        public IEnumerable<string> GetTagNames() {
            return this.GetDataModelsAs(x => x.TagName);
        }

        [Route("get_all")]
        [HttpGet]
        public IEnumerable<Tag> GetTags() {
            return this.GetDataModels();
        }

        [Route("save")]
        [HttpPost]
        public IDataTransResult SaveTags(IEnumerable<Tag> tags) {
            return this.SaveDataModels(tags, true);
        }
    }
}

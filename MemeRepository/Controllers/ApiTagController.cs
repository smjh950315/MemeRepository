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
    public class ApiTagController : MyModelAccessController, IModelHelper<Tag>
    {
        public ApiTagController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreater
            ) : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {
        }

        public IDataManager<Tag>? DefaultDataManager { get; set; }

        [Route("get_ids")]
        [HttpGet]
        public IEnumerable<long> GetTagIds() {
            return this.GetDataModelsAs(x => x.TagID, null);
        }

        //[Route("get_names")]
        //[HttpGet]
        //public IEnumerable<string> GetTagNames() {
        //    return this.GetDataModelsAs(x => x.TagName);
        //}

        [Route("get_all")]
        [HttpGet]
        public IEnumerable<Tag> GetTags() {
            return this.GetDataModels(null);
        }

        [Route("save")]
        [HttpPost]
        public IDataTransResult SaveTags(IEnumerable<Tag> tags) {
            return this.SaveDataModels(tags, null, true);
        }
    }
}

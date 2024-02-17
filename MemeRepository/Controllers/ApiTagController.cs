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
    public class ApiTagController : MyModelAccessController, IModelHelper<TAG>
    {
        public ApiTagController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreater
            ) : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {
        }

        public IDataManager<TAG>? DefaultDataManager { get; set; }

        [Route("get_ids")]
        [HttpGet]
        public IEnumerable<long> GetTagIds() {
            return this.GetDataModelsAs(x => x.ID, null);
        }

        //[Route("get_names")]
        //[HttpGet]
        //public IEnumerable<string> GetTagNames() {
        //    return this.GetDataModelsAs(x => x.TagName);
        //}

        [Route("get_all")]
        [HttpGet]
        public IEnumerable<TAG> GetTags() {
            return this.GetDataModels(null);
        }

        [Route("save")]
        [HttpPost]
        public IDataTransResult SaveTags(IEnumerable<TAG> tags) {
            return this.SaveDataModels(tags, null, true);
        }
    }
}

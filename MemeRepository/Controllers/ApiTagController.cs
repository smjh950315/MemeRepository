using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.ViewModels;
using MemeRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/tag")]
    public class ApiTagController : MyControllerBase
    {
        ITagManager _TagManager;
        public ApiTagController(
            IWebAppConfigurations webAppConfigurations,
            ITagManager tagManager
            ) : base(webAppConfigurations) {
            this._TagManager = tagManager;
        }

        //[Route("get_names")]
        //[HttpGet]
        //public IEnumerable<string> GetTagNames() {
        //    return this.GetDataModelsAs(x => x.TagName);
        //}

        [Route("get/all")]
        [HttpPost]
        public IEnumerable<TagViewModel> GetTags(IndexRange indexRange) {
            return this._TagManager.GetAll(indexRange.Begin, indexRange.Count);
        }

        [Route("save")]
        [HttpPost]
        public IDataTransResult SaveTags(IEnumerable<TagViewModel> tags) {
            return this._TagManager.Save(tags, null, true);
        }
    }
}

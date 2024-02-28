using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Lib.Interface;
using MemeRepository.Lib.ViewModels;
using MemeRepository.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class ApiCategoryController : MyControllerBase
    {
        ICateManager _CateManager;
        public ApiCategoryController(
            IWebAppConfigurations webAppConfigurations,
            ICateManager cateManager
            ) : base(webAppConfigurations) {
            this._CateManager = cateManager;
        }


        [Route("category/get_all")]
        [HttpPost]
        public IEnumerable<CategoryViewModel> GetCategory(IndexRange indexRange) {
            return this._CateManager.GetAll(indexRange.Begin, indexRange.Count);
        }

    }
}

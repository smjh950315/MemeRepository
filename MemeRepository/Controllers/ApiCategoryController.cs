using Cyh.DataHelper;
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
    public class ApiCategoryController : MyModelAccessController
    {
        ICateManager _CateManager;
        public ApiCategoryController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreater,
            ICateManager cateManager
            ) : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {
            this._CateManager = cateManager;
        }


        [Route("category/get_all")]
        [HttpPost]
        public IEnumerable<CategoryViewModel> GetCategory(IndexRange indexRange) {
            return this._CateManager.GetAll(indexRange.Begin, indexRange.Count);
        }

        //[Route("category/Add_category")]
        //[HttpPost]
        //public IDataTransResult AddCategory(string categoryName) {
        //    return this.SaveDataModels(
        //        new DataModel[]{
        //            new DataModel(){
        //                CategoryName = categoryName,
        //                CreateTime = DateTime.Now,
        //                UpdateTime = DateTime.Now,
        //            }
        //    }, null, true);
        //}
    }
}

using Cyh.DataHelper;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using DataModel = MemeRepository.Db.Models.Category;
using Microsoft.AspNetCore.Mvc;
using Cyh.DataModels;
using Cyh.Modules.ModViewData;

namespace MemeRepository.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class ApiCategoryController : MyModelAccessController, IModelHelper<DataModel>
    {
        public ApiCategoryController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreater
            ) : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {
        }

        public IDataManager<DataModel>? DefaultDataManager { get; set; }

        [Route("category/get_all")]
        [HttpGet]
        public IEnumerable<DataModel> GetCategory() {
            return this.GetDataModels(null);
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

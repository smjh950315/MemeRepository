using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Controller;
using MemeRepository.Db.Models;
using System.Linq.Expressions;

namespace MemeRepository.Controllers
{
    public class DemoController : MyModelAccessController, IModelHelper<TAG>
    {
        public IDataManager<TAG>? DefaultDataManager { get; set; }
        public DemoController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreater
            ) : base(webAppConfigurations, dataManagerActivator, dataManagerCreater) {
        }
        private void GetSetDataDemos<T>() where T : class {
            Expression<Func<T, TAG>>? selectorToData = null;
            Expression<Func<TAG, T>>? selectorFromData = null;
            Expression<Func<TAG, bool>>? filter = null;
            TAG dataModel = null;
            IEnumerable<TAG> dataModels = null;
            T viewModel = null;
            IEnumerable<T> viewModels = null;
            IDataTransResult dataTransResult = null;
            object? nuknow = null;

            IModelHelper<TAG> modelHelper = this;
            modelHelper.GetDataModel(filter);
            modelHelper.GetDataModels(0, 1, filter);
            modelHelper.GetDataModelAs(selectorFromData, filter);
            modelHelper.GetDataModelsAs(selectorFromData, 0, 1, filter);
            modelHelper.SaveDataModel(dataModel, dataTransResult, false);
            modelHelper.SaveDataModels(dataModels, dataTransResult, false);
            modelHelper.SaveDataModelFrom(selectorToData, viewModel, dataTransResult, false);
            modelHelper.SaveDataModelsFrom(selectorToData, viewModels, dataTransResult, false);

            IDataManager<TAG>? mgr = modelHelper.GetDataManager<TAG>();
            mgr.GetData(filter, dataTransResult);
            mgr.GetDatas(0, 1, filter, dataTransResult);
            mgr.GetDataAs(selectorFromData, filter, dataTransResult);
            mgr.GetDatasAs(selectorFromData, filter, dataTransResult);
            mgr.SaveData(dataModel, dataTransResult, false);
            mgr.SaveDataFrom(selectorToData, null, dataTransResult, false);

            IMyDataAccesser<TAG>? myDataAccesser = mgr?.MainDataSource;
            myDataAccesser?.TryGetData(filter, dataTransResult);
            myDataAccesser?.TryGetDatas(filter, dataTransResult);
            myDataAccesser?.TryGetDatas(0, 1, filter, dataTransResult);
            myDataAccesser?.TryAddOrUpdate(Enumerable.Empty<T>(), dataTransResult, false);
            myDataAccesser?.TryAddOrUpdate(Enumerable.Empty<TAG>(), dataTransResult, false);
            myDataAccesser?.TryAddOrUpdate(dataModel, dataTransResult, false);

            myDataAccesser?.TryGetDataAs(selectorFromData, filter, dataTransResult);
            myDataAccesser?.TryGetDatasAs(selectorFromData, 0, 1, filter, dataTransResult);
        }
    }
}

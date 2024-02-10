
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 物件模型存取工具的基底介面
    /// </summary>
    public interface IModelHelper
    {
        /// <summary>
        /// 資料管理器設定器
        /// </summary>
        IDataManagerActivator DataManagerActivator { get; }

        /// <summary>
        /// 資料管理器(未初始化)產生器
        /// </summary>
        IDataManagerCreater DataManagerCreater { get; }

        /// <summary>
        /// 空的資料交換結果
        /// </summary>
        IDataTransResult EmptyResult { get; }

        /// <summary>
        /// 主要的模型資訊
        /// </summary>
        Type? ModelType { get; }
    }

    /// <summary>
    /// DataModel 存取工具的基底介面
    /// </summary>
    public interface IModelHelper<DataModel> : IModelHelper where DataModel : class
    {
        /// <summary>
        /// 預設的主要資料管理器
        /// </summary>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        IDataManager<DataModel>? DefaultDataManager { get; set; }
    }

    public static partial class MyDataHelperExtends
    {
        private static IDataManager<DataModel>? __UnChecked_CreateAndActivateNewDataManager<DataModel>(IModelHelper modelHelper) where DataModel : class {
            IDataManager<DataModel>? newDataManager = modelHelper.DataManagerCreater.GetDefault<DataModel>();
            if (newDataManager == null) {
                throw new NotImplementedException("DataManagerCreater is not be correctly implemented !");
            }
            modelHelper.DataManagerActivator.Activate(newDataManager);
            return newDataManager;
        }

        /// <summary>
        /// 取得主要資料管理器並活性化(即設定資料源)
        /// </summary>
        /// <typeparam name="DataModel">要管理的資料模型</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        public static IDataManager<DataModel>? GetDataManager<DataModel>(this IModelHelper? modelHelper) where DataModel : class {
            if (modelHelper == null) return null;

            IDataManager<DataModel>? retManager;
            if (modelHelper is IModelHelper<DataModel> thisTypeHelper) {
                thisTypeHelper.DefaultDataManager ??= __UnChecked_CreateAndActivateNewDataManager<DataModel>(modelHelper);
                retManager = thisTypeHelper.DefaultDataManager;
            } else {
                retManager = __UnChecked_CreateAndActivateNewDataManager<DataModel>(modelHelper);
            }
            return retManager;
        }

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        public static DataModel? GetDataModel<DataModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<DataModel, bool>> expression) where DataModel : class {
            return modelHelper
                .GetDataManager<DataModel>()
                .GetData(expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public static IEnumerable<DataModel> GetDataModels<DataModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<DataModel, bool>>? expression) where DataModel : class {
            return modelHelper
                .GetDataManager<DataModel>()
                .GetDatas(expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="begin">開始的索引</param>
        /// <param name="count">最大取得的數量</param>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public static IEnumerable<DataModel> GetDataModels<DataModel>(this IModelHelper<DataModel>? modelHelper, int begin, int count, Expression<Func<DataModel, bool>>? expression) where DataModel : class {
            return modelHelper
                .GetDataManager<DataModel>()
                .GetDatas(begin, count, expression, null);
        }

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="selector">轉換到目標模型的函數</param>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        public static TOut? GetDataModelAs<TOut, DataModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>>? expression) where DataModel : class {
            return modelHelper
                .GetDataManager<DataModel>()
                .GetDataAs(selector, expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="selector">轉換到目標模型的函數</param>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public static IEnumerable<TOut> GetDataModelsAs<TOut, DataModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>>? expression) where DataModel : class {
            return modelHelper
                .GetDataManager<DataModel>()
                .GetDatasAs(selector, expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="selector">轉換到目標模型的函數</param>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public static IEnumerable<TOut> GetDataModelsAs<TOut, DataModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<DataModel, TOut>>? selector, int begin, int count, Expression<Func<DataModel, bool>>? expression) where DataModel : class {
            return modelHelper
                .GetDataManager<DataModel>()
                .GetDatasAs(selector, begin, count, expression, null);
        }

        /// <summary>
        /// 儲存資料模型
        /// </summary>
        /// <param name="dataModel">資料模型</param>
        /// <param name="dataTransResult">交易執行的結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult SaveDataModel<DataModel>(this IModelHelper<DataModel>? modelHelper, DataModel? dataModel, IDataTransResult? dataTransResult, bool execNow) where DataModel : class {
            IDataTransResult result = dataTransResult ?? modelHelper?.EmptyResult ?? new DataTransResultBase();
            if (dataModel == null)
                return result;
            IDataManager<DataModel>? dataManager = modelHelper?.GetDataManager<DataModel>();
            if (dataManager == null)
                return result;
            dataManager.SaveData(dataModel, result, execNow);
            return result;
        }

        /// <summary>
        /// 儲存資料模型集合
        /// </summary>
        /// <param name="dataModels">資料模型集合</param>
        /// <param name="dataTransResult">交易執行的結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult SaveDataModels<DataModel>(this IModelHelper<DataModel>? modelHelper, IEnumerable<DataModel> dataModels, IDataTransResult? dataTransResult, bool execNow) where DataModel : class {
            IDataTransResult result = dataTransResult ?? modelHelper?.EmptyResult ?? new DataTransResultBase();
            if (dataModels.IsNullOrEmpty())
                return result;
            IDataManager<DataModel>? dataManager = modelHelper?.GetDataManager<DataModel>();
            if (dataManager == null)
                return result;
            dataManager.SaveDatas(dataModels, result, execNow);
            return result;
        }

        /// <summary>
        /// 儲存資料模型
        /// </summary>
        /// <param name="dataModel">資料模型</param>
        /// <param name="dataTransResult">交易執行的結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult SaveDataModelFrom<DataModel, InputModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<InputModel, DataModel>>? selector, InputModel? dataModel, IDataTransResult? dataTransResult, bool execNow) where DataModel : class {
            IDataTransResult result = dataTransResult ?? modelHelper?.EmptyResult ?? new DataTransResultBase();
            if (dataModel == null)
                return result;
            IDataManager<DataModel>? dataManager = modelHelper?.GetDataManager<DataModel>();
            if (dataManager == null)
                return result;
            dataManager.SaveDataFrom(selector, dataModel, result, execNow);
            return result;
        }

        /// <summary>
        /// 儲存資料模型集合
        /// </summary>
        /// <param name="dataModels">資料模型集合</param>
        /// <param name="dataTransResult">交易執行的結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult SaveDataModelsFrom<DataModel, InputModel>(this IModelHelper<DataModel>? modelHelper, Expression<Func<InputModel, DataModel>>? selector, IEnumerable<InputModel> dataModels, IDataTransResult? dataTransResult, bool execNow) where DataModel : class {
            IDataTransResult result = dataTransResult ?? modelHelper?.EmptyResult ?? new DataTransResultBase();
            if (dataModels.IsNullOrEmpty())
                return result;
            IDataManager<DataModel>? dataManager = modelHelper?.GetDataManager<DataModel>();
            if (dataManager == null)
                return result;
            dataManager.SaveDatasFrom(selector, dataModels, result, execNow);
            return result;
        }
    }
}

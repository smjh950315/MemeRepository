
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 以 ViewModel 存取 DataModel 工具的基底介面
    /// </summary>
    public interface IViewModelAccesser
    {
        /// <summary>
        /// 資料管理器設定器
        /// </summary>
        public IDataManagerActivator DataManagerActivator { get; }

        /// <summary>
        /// 空的資料交換結果
        /// </summary>
        IDataTransResult EmptyResult { get; }
    }

    /// <summary>
    /// 以 ViewModel 存取 DataModel工具 的基底介面
    /// </summary>
    public interface IViewModelAccesser<DataModel> : IViewModelAccesser
    {
        /// <summary>
        /// 取得主要資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        public TManager? GetManager<TManager>() where TManager : class, IDataManager<DataModel>;

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        public DataModel? GetDataModel(Expression<Func<DataModel, bool>> expression);

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<DataModel> GetDataModels(Expression<Func<DataModel, bool>>? expression = null);

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<DataModel> GetDataModels(int begin, int count, Expression<Func<DataModel, bool>>? expression = null);

        /// <summary>
        /// 儲存資料模型集合
        /// </summary>
        /// <param name="dataModels">資料模型集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public IDataTransResult SaveDataModels(IEnumerable<DataModel> dataModels, IDataTransResult? transResult = null, bool execNow = false);

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        public TOut? GetDataModelAs<TOut>(Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>> expression);

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<TOut> GetDataModelsAs<TOut>(Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>>? expression = null);

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<TOut> GetDataModelsAs<TOut>(Expression<Func<DataModel, TOut>>? selector, int begin, int count, Expression<Func<DataModel, bool>>? expression = null);
    }

    /// <summary>
    /// 以 ViewModel 存取 DataModel 的介面，唯 ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    public interface IViewModelAccesser<DataModel, ViewModel, IModel> : IViewModelAccesser<DataModel>
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
    {
        /// <summary>
        /// 用特定條件從資料管理器取得指定ViewModel(轉換自DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的ViewModel</returns>
        public ViewModel? GetViewModel(Expression<Func<DataModel, bool>> expression);

        /// <summary>
        /// 用特定條件從資料管理器取得指定ViewModel(轉換自DataModel)
        /// </summary>
        /// <param name="begin">開始的索引</param>
        /// <param name="count">要取得的最大數量</param>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的ViewModel集合</returns>
        public IEnumerable<ViewModel> GetViewModels(int begin, int count, Expression<Func<DataModel, bool>>? expression = null);

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModel">要儲存的DataModel</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public IDataTransResult SaveFromViewModel(ViewModel viewModel, IDataTransResult? transResult = null, bool execNow = false);

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModels">要儲存的DataModel集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public IDataTransResult SaveFromViewModels(IEnumerable<ViewModel> viewModels, IDataTransResult? transResult = null, bool execNow = false);
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 取得初始化的資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器的類型</typeparam>
        /// <typeparam name="TDataModel">要管理的資料模型</typeparam>
        /// <param name="dataManagerCreaterBase">資料管理器(未初始化)產生器</param>
        /// <param name="manager">要設定的資料管理器實體</param>
        /// <returns></returns>
        public static TManager? GetDataManager<TManager, TDataModel>(
            this IViewModelAccesser castableModelController,
            IDataManagerCreater dataManagerCreaterBase,
            ref IDataManager<TDataModel>? manager)
            where TManager : class, IDataManager<TDataModel>
            where TDataModel : class {
            if (manager == null) {
                manager = dataManagerCreaterBase.GetDefault<TDataModel>() as TManager;
                castableModelController.DataManagerActivator.Activate(manager);
            }
            return manager as TManager;
        }
    }
}

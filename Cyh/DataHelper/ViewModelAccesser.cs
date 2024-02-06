
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 以 ViewModel 存取 DataModel 工具的基底實作
    /// </summary>
    public class ViewModelAccesser : IViewModelAccesser
    {
        // 存取者名稱(或ID)
        internal string? _Accesser;

        // 資料管理器設定器
        internal IDataManagerActivator _DataManagerActivator;

        // 介面實作(資料管理器設定器)
        public IDataManagerActivator DataManagerActivator => this._DataManagerActivator;

        // 空的資料交換結果(實作)(初始化時設定時間、存取者)
        public IDataTransResult EmptyResult => new DataTransResultBase()
        {
            BeginTime = DateTime.Now,
            Accesser = this._Accesser ?? "UNKNOW",
        };

        public ViewModelAccesser(
            IDataManagerActivator dataManagerActivator) {
            this._DataManagerActivator = dataManagerActivator;
        }
    }

    /// <summary>
    /// 以 ViewModel 存取 DataModel 工具的基底實作
    /// </summary>
    public class ViewModelAccesser<DataModel> : ViewModelAccesser, IViewModelAccesser<DataModel> where DataModel : class
    {
        /// <summary>
        /// 資料管理器(未初始化)產生器
        /// </summary>
        protected IDataManagerCreater _ThisManagerCreaterBase;

        /// <summary>
        /// 資料管理器實體
        /// </summary>
        IDataManager<DataModel>? _ThisManagerInstance;

        /// <summary>
        /// 空的DataModel集合
        /// </summary>
        protected IEnumerable<DataModel> EmptyDataModels => Enumerable.Empty<DataModel>();

        public ViewModelAccesser(
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreaterBase)
            : base(dataManagerActivator) {
            this._ThisManagerCreaterBase = dataManagerCreaterBase;
        }

        /// <summary>
        /// 取得主要資料管理器並活性化(即設定資料源)
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        public TManager? GetManager<TManager>() where TManager : class, IDataManager<DataModel> {
            return this.GetDataManager<TManager, DataModel>(this._ThisManagerCreaterBase, ref this._ThisManagerInstance);
        }

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        public DataModel? GetDataModel(Expression<Func<DataModel, bool>> expression) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetData(expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<DataModel> GetDataModels(Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatas(expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<DataModel> GetDataModels(int begin, int count, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatas(begin, count, expression, null);
        }

        /// <summary>
        /// 儲存資料模型集合
        /// </summary>
        /// <param name="dataModels">資料模型集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public IDataTransResult SaveDataModels(IEnumerable<DataModel> dataModels, IDataTransResult? transResult = null, bool execNow = false) {
            IDataTransResult result = transResult ?? this.EmptyResult;
            if (dataModels.IsNullOrEmpty())
                return result;
            IDataManager<DataModel>? dataManager = this.GetManager<IDataManager<DataModel>>();
            if (dataManager == null)
                return result;
            dataManager.SaveDatas(dataModels, result, execNow);
            return result;
        }

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        public TOut? GetDataModelAs<TOut>(Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>> expression) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDataAs(selector, expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<TOut> GetDataModelsAs<TOut>(Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatasAs(selector, expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        public IEnumerable<TOut> GetDataModelsAs<TOut>(Expression<Func<DataModel, TOut>>? selector, int begin, int count, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatasAs(selector, begin, count, expression, null);
        }
    }

    /// <summary>
    /// 以 ViewModel 存取 DataModel 的介面，唯 ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    public class ViewModelAccesser<DataModel, ViewModel, IModel> : ViewModelAccesser<DataModel>, IViewModelAccesser<DataModel, ViewModel, IModel>
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
    {
        public IEnumerable<ViewModel> EmptyViewModels => Enumerable.Empty<ViewModel>();

        public ViewModelAccesser(
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定ViewModel(轉換自DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的ViewModel</returns>
        public ViewModel? GetViewModel(Expression<Func<DataModel, bool>> expression) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDataAs(new ViewModel().GetSelectorFrom<DataModel>(), expression, null);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定ViewModel(轉換自DataModel)
        /// </summary>
        /// <param name="begin">開始的索引</param>
        /// <param name="count">要取得的最大數量</param>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的ViewModel集合</returns>
        public IEnumerable<ViewModel> GetViewModels(int begin, int count, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatasAs(new ViewModel().GetSelectorFrom<DataModel>(), begin, count, expression, null);
        }

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModel">要儲存的DataModel</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public IDataTransResult SaveFromViewModel(ViewModel viewModel, IDataTransResult? transResult = null, bool execNow = false) {
            IDataTransResult result = transResult ?? this.EmptyResult;
            if (viewModel == null)
                return result;
            IDataManager<DataModel>? mainManager = this.GetManager<IDataManager<DataModel>>();
            if (mainManager == null)
                return result;
            mainManager.SaveDataFrom(new ViewModel().GetSelectorTo<DataModel>(), viewModel, result, execNow);
            return result;
        }

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModels">要儲存的DataModel集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public IDataTransResult SaveFromViewModels(IEnumerable<ViewModel> viewModels, IDataTransResult? transResult = null, bool execNow = false) {
            IDataTransResult result = transResult ?? this.EmptyResult;
            if (viewModels.IsNullOrEmpty())
                return result;
            IDataManager<DataModel>? mainManager = this.GetManager<IDataManager<DataModel>>();
            if (mainManager == null)
                return result;
            mainManager.SaveDatasFrom(new ViewModel().GetSelectorTo<DataModel>(), viewModels, result, execNow);
            return result;
        }
    }
}

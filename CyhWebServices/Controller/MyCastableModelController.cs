using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using System.Linq.Expressions;

namespace Cyh.WebServices.Controller
{
    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器基底介面
    /// </summary>
    public interface IMyCastableModelController
    {
        /// <summary>
        /// 資料管理器活性化設定器
        /// </summary>
        IDataManagerActivator DataManagerActivator { get; }

        /// <summary>
        /// 空的資料交換結果
        /// </summary>
        IDataTransResult EmptyResult { get; }
    }

    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器，ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    public interface IMyCastableModelController<DataModel, ViewModel, IModel> : IMyCastableModelController
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
    {
        /// <summary>
        /// 取得資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        TManager? GetManager<TManager>() where TManager : class, IDataManager<DataModel>;
    }

    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器基本實作，ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    public class MyCastableModelController : MyControllerBase, IMyCastableModelController
    {
        internal IDataManagerActivator _DataManagerActivator;
        public IDataManagerActivator DataManagerActivator => this._DataManagerActivator;
        protected MyCastableModelController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator)
            : base(webAppConfigurations) {
            this._DataManagerActivator = dataManagerActivator;
        }

        protected IViewModelAccesser GetCustomModelAccerss() {
            return new ViewModelAccesser(this._DataManagerActivator);
        }
    }

    public class MyCastableModelController<DataModel> : MyCastableModelController where DataModel : class
    {
        protected IDataManagerCreater _ThisManagerCreaterBase;
        IDataManager<DataModel>? _ThisManager;
        protected IEnumerable<DataModel> EmptyDataModels => Enumerable.Empty<DataModel>();
        protected MyCastableModelController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreaterBase)
            : base(webAppConfigurations, dataManagerActivator) {
            this._ThisManagerCreaterBase = dataManagerCreaterBase;
        }

        /// <summary>
        /// 取得主要資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        protected TManager? GetManager<TManager>() where TManager : class, IDataManager<DataModel> {
            return this.GetDataManager<TManager, DataModel>(this._ThisManagerCreaterBase, ref this._ThisManager);
        }

        /// <summary>
        /// 取得客製資料管理器並活性化(即設定資料源)
        /// </summary>
        /// <typeparam name="TModel">資料模型</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        public IDataManager<TModel>? GetCustManager<TModel>() where TModel : class {
            IDataManager<TModel>? retVal = null;
            return this.GetDataManager<IDataManager<TModel>, TModel>(this._ThisManagerCreaterBase, ref retVal);
        }

        /// <summary>
        /// 取得資料模型
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型</returns>
        protected DataModel? GetDataModel(Expression<Func<DataModel, bool>> expression) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetData(expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        protected IEnumerable<DataModel> GetDataModels(Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatas(expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        protected IEnumerable<DataModel> GetDataModels(int begin, int count, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatas(begin, count, expression, null);
        }

        ///// <summary>
        ///// 儲存資料模型集合
        ///// </summary>
        ///// <param name="dataModels">資料模型集合</param>
        ///// <param name="execNow">是否立即執行</param>
        ///// <returns>執行結果</returns>
        protected IDataTransResult SaveDataModels(IEnumerable<DataModel> dataModels, IDataTransResult? transResult = null, bool execNow = false) {
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
        protected TOut? GetDataModelAs<TOut>(Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>> expression) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDataAs(selector, expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        protected IEnumerable<TOut> GetDataModelsAs<TOut>(Expression<Func<DataModel, TOut>>? selector, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatasAs(selector, expression, null);
        }

        /// <summary>
        /// 取得資料模型集合
        /// </summary>
        /// <param name="expression">條件式</param>
        /// <returns>資料模型集合</returns>
        protected IEnumerable<TOut> GetDataModelsAs<TOut>(Expression<Func<DataModel, TOut>>? selector, int begin, int count, Expression<Func<DataModel, bool>>? expression = null) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetDatasAs(selector, begin, count, expression, null);
        }
    }

    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器基本實作，ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    public class MyCastableModelController<DataModel, ViewModel, IModel> : MyCastableModelController<DataModel>
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
    {
        protected IEnumerable<ViewModel> EmptyViewModels => Enumerable.Empty<ViewModel>();
        protected MyCastableModelController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreaterBase)
            : base(webAppConfigurations, dataManagerActivator, dataManagerCreaterBase) {
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定ViewModel(轉換自DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的ViewModel</returns>
        protected ViewModel? GetViewModel(Expression<Func<DataModel, bool>> expression) {
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
        protected IEnumerable<ViewModel> GetViewModels(int begin, int count, Expression<Func<DataModel, bool>>? expression = null) {
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
        protected IDataTransResult SaveFromViewModel(ViewModel viewModel, IDataTransResult? transResult = null, bool execNow = false) {
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
        protected IDataTransResult SaveFromViewModels(IEnumerable<ViewModel> viewModels, IDataTransResult? transResult = null, bool execNow = false) {
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

    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器基本實作，ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">主要DataModel</typeparam>
    /// <typeparam name="ViewModel">主要ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    /// <typeparam name="SubDataModel">次要DataModel</typeparam>
    /// <typeparam name="SubViewModel">次要ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="ISubModel">次要DataModel與次要ViewModel的共同介面</typeparam>
    public class MyCastableModelController
        <DataModel, ViewModel, IModel, SubDataModel, SubViewModel, ISubModel>
        : MyCastableModelController<DataModel, ViewModel, IModel>
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
        where SubDataModel : class, ISubModel, new()
        where SubViewModel : class, ISubModel, ISelectableModel<SubViewModel, ISubModel>, new()
    {
        IDataManager<SubDataModel>? _ThisManager;
        protected MyCastableModelController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManagerCreater dataManagerCreaterBase)
            : base(webAppConfigurations, dataManagerActivator, dataManagerCreaterBase) {
        }

        /// <summary>
        /// 取得次要資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        protected TManager? GetSubManager<TManager>() where TManager : class, IDataManager<SubDataModel> {
            return this.GetDataManager<TManager, SubDataModel>(this._ThisManagerCreaterBase, ref this._ThisManager);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定次要ViewModel(轉換自次要DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的次要ViewModel</returns>
        protected SubViewModel? GetSubViewModel(Expression<Func<SubDataModel, bool>> expression) {
            return this
                .GetSubManager<IDataManager<SubDataModel>>()
                .GetDataAs(new SubViewModel().GetSelectorFrom<SubDataModel>(), expression, null);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定次要ViewModel(轉換自次要DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的次要ViewModel集合</returns>
        protected IEnumerable<SubViewModel> GetSubViewModels(Expression<Func<SubDataModel, bool>>? expression = null) {
            return this
                .GetSubManager<IDataManager<SubDataModel>>()
                .GetDatasAs(new SubViewModel().GetSelectorFrom<SubDataModel>(), expression, null);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定次要ViewModel(轉換自次要DataModel)
        /// </summary>
        /// <param name="begin">開始的索引</param>
        /// <param name="count">要取得的最大數量</param>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的次要ViewModel集合</returns>
        protected IEnumerable<SubViewModel> GetSubViewModels(int begin, int count, Expression<Func<SubDataModel, bool>>? expression = null) {
            return this
                .GetSubManager<IDataManager<SubDataModel>>()
                .GetDatasAs(new SubViewModel().GetSelectorFrom<SubDataModel>(), begin, count, expression, null);
        }

        /// <summary>
        /// 儲存次要DataModel(轉換自次要ViewModel)
        /// </summary>
        /// <param name="viewModel">要儲存的次要DataModel</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult SaveFromSubViewModel(SubViewModel viewModel, IDataTransResult? transResult = null, bool execNow = false) {
            IDataTransResult result = transResult ?? this.EmptyResult;
            if (viewModel == null)
                return result;
            IDataManager<SubDataModel>? subManager = this.GetSubManager<IDataManager<SubDataModel>>();
            if (subManager == null)
                return result;
            subManager.SaveDataFrom(new SubViewModel().GetSelectorTo<SubDataModel>(), viewModel, result, execNow);
            return result;
        }

        /// <summary>
        /// 儲存次要DataModel(轉換自次要ViewModel)
        /// </summary>
        /// <param name="viewModels">要儲存的次要DataModel集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult SaveFromSubViewModels(IEnumerable<SubViewModel> viewModels, IDataTransResult? transResult = null, bool execNow = false) {
            IDataTransResult result = transResult ?? this.EmptyResult;
            if (viewModels.IsNullOrEmpty())
                return result;
            IDataManager<SubDataModel>? subManager = this.GetSubManager<IDataManager<SubDataModel>>();
            if (subManager == null)
                return result;
            subManager.SaveDatasFrom(new SubViewModel().GetSelectorTo<SubDataModel>(), viewModels, result, execNow);
            return result;
        }
    }
}

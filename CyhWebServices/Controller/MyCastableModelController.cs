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
    }

    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器，ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    public interface IMyCastableModelController<DataModel, ViewModel, IModel>
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
    {
        /// <summary>
        /// 空的資料交換結果
        /// </summary>
        IDataTransResult EmptyResult { get; }

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
    }

    /// <summary>
    /// 可以在 DataModel 與 ViewModel 間轉換存取的控制器基本實作，ViewModel 必須先繼承 ISelectableModel 並實作轉換函數
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel，必須先繼承 ISelectableModel 並實作轉換函數</typeparam>
    /// <typeparam name="IModel">DataModel與ViewModel的共同介面</typeparam>
    public class MyCastableModelController<DataModel, ViewModel, IModel> : MyCastableModelController
        where DataModel : class, IModel, new()
        where ViewModel : class, IModel, ISelectableModel<ViewModel, IModel>, new()
    {
        IDataManager _ThisManagerBase;
        IDataManager<DataModel>? _ThisManager;
        protected IEnumerable<ViewModel> EmptyViewModels => Enumerable.Empty<ViewModel>();
        protected MyCastableModelController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManager dataManagerBase)
            : base(webAppConfigurations, dataManagerActivator) {
            this._ThisManagerBase = dataManagerBase;
        }

        /// <summary>
        /// 取得主要資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        protected TManager? GetManager<TManager>() where TManager : class, IDataManager<DataModel> {
            return this.GetDataManager<TManager, DataModel>(this._ThisManagerBase, ref this._ThisManager);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定ViewModel(轉換自DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的ViewModel</returns>
        protected ViewModel? GetViewModel(Expression<Func<DataModel, bool>> expression) {
            return this
                .GetManager<IDataManager<DataModel>>()
                .GetMainFormAs(new ViewModel().GetSelectorFrom<DataModel>(), expression);
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
                .GetMainFormsAs(begin, count, new ViewModel().GetSelectorFrom<DataModel>(), expression);
        }

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModels">要儲存的DataModel集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult SaveFromViewModels(IEnumerable<ViewModel> viewModels, bool execNow = false) {
            if (viewModels.IsNullOrEmpty())
                return this.EmptyResult;
            IDataManager<DataModel>? mainManager = this.GetManager<IDataManager<DataModel>>();
            if (mainManager == null)
                return this.EmptyResult;
            IDataTransResult result = this.EmptyResult;
            return mainManager.SaveMainForms(new ViewModel().GetSelectorTo<DataModel>(), viewModels, result, execNow) ?? this.EmptyResult;
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
        IDataManager _ThisManagerBase;
        IDataManager<SubDataModel>? _ThisManager;
        protected MyCastableModelController(
            IWebAppConfigurations webAppConfigurations,
            IDataManagerActivator dataManagerActivator,
            IDataManager dataManagerBase)
            : base(webAppConfigurations, dataManagerActivator, dataManagerBase) {
            this._ThisManagerBase = dataManagerBase;
        }

        /// <summary>
        /// 取得次要資料管理器
        /// </summary>
        /// <typeparam name="TManager">資料管理器</typeparam>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        protected TManager? GetSubManager<TManager>() where TManager : class, IDataManager<SubDataModel> {
            return this.GetDataManager<TManager, SubDataModel>(this._ThisManagerBase, ref this._ThisManager);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定次要ViewModel(轉換自次要DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的次要ViewModel</returns>
        protected SubViewModel? GetSubViewModel(Expression<Func<SubDataModel, bool>> expression) {
            return this
                .GetSubManager<IDataManager<SubDataModel>>()
                .GetMainFormAs(new SubViewModel().GetSelectorFrom<SubDataModel>(), expression);
        }

        /// <summary>
        /// 用特定條件從資料管理器取得指定次要ViewModel(轉換自次要DataModel)
        /// </summary>
        /// <param name="expression">過濾條件</param>
        /// <returns>取得的次要ViewModel集合</returns>
        protected IEnumerable<SubViewModel> GetSubViewModels(Expression<Func<SubDataModel, bool>>? expression = null) {
            return this
                .GetSubManager<IDataManager<SubDataModel>>()
                .GetMainFormsAs(new SubViewModel().GetSelectorFrom<SubDataModel>(), expression);
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
                .GetMainFormsAs(begin, count, new SubViewModel().GetSelectorFrom<SubDataModel>(), expression);
        }

        /// <summary>
        /// 儲存次要DataModel(轉換自次要ViewModel)
        /// </summary>
        /// <param name="viewModels">要儲存的次要DataModel集合</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult SaveFromSubViewModels(IEnumerable<SubViewModel> viewModels, bool execNow = false) {
            if (viewModels.IsNullOrEmpty())
                return this.EmptyResult;
            IDataManager<SubDataModel>? subManager = this.GetSubManager<IDataManager<SubDataModel>>();
            if (subManager == null)
                return this.EmptyResult;
            IDataTransResult result = this.EmptyResult;
            return subManager.SaveMainForms(new SubViewModel().GetSelectorTo<SubDataModel>(), viewModels, result, execNow) ?? this.EmptyResult;
        }
    }
}

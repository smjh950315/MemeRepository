using Cyh.DataHelper;
using Cyh.DataModels;
using Cyh.Modules.ModForm;
using System.Linq.Expressions;

namespace Cyh.Modules.ModViewData
{
    /// <summary>
    /// 一個主資料與其關聯的多個副資料，以檢視用的模型進行存取的輔助類別
    /// </summary>
    /// <typeparam name="TMainDataModel">主資料模型</typeparam>
    /// <typeparam name="TMainViewModel">主檢視模型</typeparam>
    /// <typeparam name="TSubDataModel">副資料模型</typeparam>
    /// <typeparam name="TSubViewModel">副檢視模型</typeparam>
    public abstract class MyRelatedViewModelHelperBase<TMainDataModel, TMainViewModel, TSubDataModel, TSubViewModel>
        : MyViewModelHelperBase<TMainDataModel, TMainViewModel>
        , IViewModelHelper<TSubDataModel, TSubViewModel>
        , IRelatedModelHelper<TMainDataModel, TSubDataModel>
        where TMainDataModel : class
        where TMainViewModel : class
        where TSubDataModel : class
        where TSubViewModel : class
    {
        protected MyRelatedViewModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }
        public IViewModelHelper<TSubDataModel, TSubViewModel> SubModelHelper => this;
        IDataManager<TSubDataModel>? IModelHelper<TSubDataModel>.DefaultDataManager { get; set; }

        #region IViewModelHelper<TMainDataModel, TMainViewModel> 的成員
        /// <summary>
        /// 將檢視模型轉換為資料模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        //public abstract Expression<Func<TMainViewModel, TMainDataModel>> GetExprToDataModel();

        /// <summary>
        /// 將資料模型轉換為檢視模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        //public abstract Expression<Func<TMainDataModel, TMainViewModel>> GetExprToViewModel();

        /// <summary>
        /// 用來查詢與輸入資料相關的資料模型取得函數
        /// </summary>
        /// <param name="view">檢視模型</param>
        /// <returns>查詢用的敘述式</returns>
        //public abstract Expression<Func<TMainDataModel, bool>> GetExprToFindDataModel(TMainViewModel view);

        /// <summary>
        /// 用資料的檢視模型來更新資料模型
        /// </summary>
        /// <param name="data">資料模型</param>
        /// <param name="view">檢視模型</param>
        //public abstract void UpdateModelFromViewModel(TMainDataModel data, TMainViewModel view);
        #endregion

        #region IViewModelHelper<TSubDataModel, TSubViewModel> 的成員
        #region 由於無法被子類別存取，因此需要額外呼叫與子類別的接口方法
        Expression<Func<TSubViewModel, TSubDataModel>> IViewModelHelper<TSubDataModel, TSubViewModel>.GetExprToDataModel() {
            return this.GetExprToSubDataModel();
        }
        Expression<Func<TSubDataModel, TSubViewModel>> IViewModelHelper<TSubDataModel, TSubViewModel>.GetExprToViewModel() {
            return this.GetExprToSubViewModel();
        }
        #endregion

        public abstract Expression<Func<TSubDataModel, bool>> GetExprToFindDataModel(TSubViewModel view);
        public abstract void UpdateModelFromViewModel(TSubDataModel data, TSubViewModel view);
        #endregion

        #region IRelatedModelHelper<TMainDataModel, TSubDataModel> 的成員
        /// <summary>
        /// 用來查詢與輸入主資料相關的副資料模型取得函數
        /// </summary>
        /// <param name="data">主資料模型</param>
        /// <returns>查詢用的敘述式</returns>
        public abstract Expression<Func<TSubDataModel, bool>> GetExprRelateToMainData(TMainDataModel data);
        #endregion

        #region 由於介面 IViewModelHelper<TSubDataModel, TSubViewModel> 的 GetExprTo... 方法無法被子類別取得，因此由此二抽象方法作為與子類別的接口
        /// <summary>
        /// 將檢視模型轉換為資料模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        public abstract Expression<Func<TSubViewModel, TSubDataModel>> GetExprToSubDataModel();

        /// <summary>
        /// 將資料模型轉換為檢視模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        public abstract Expression<Func<TSubDataModel, TSubViewModel>> GetExprToSubViewModel();
        #endregion


        /// <summary>
        /// 用主資料模型查詢式取得檢視資料組
        /// </summary>
        /// <typeparam name="TViewGroup">檢視資料組</typeparam>
        /// <param name="expression">查詢式</param>
        /// <returns>檢視資料組，如果沒有符合的結果回傳null</returns>
        protected virtual TViewGroup? GetViewGroup<TViewGroup>(Expression<Func<TMainDataModel, bool>> expression)
            where TViewGroup : class, IFormGroup<TMainViewModel, TSubViewModel>, new() {
            if (!this.MainModelHelper.Exist(expression)) { return null; }
            TMainDataModel? mainData = this.MainModelHelper.GetDataModel(expression);
            if (mainData == null) { return null; }

            TMainViewModel? mainView = ConvertByExpr(this.GetExprToViewModel(), mainData);
            if (mainView == null) { return null; }

            IEnumerable<TSubViewModel> subViews = this.SubModelHelper.GetDataModelsAs(
                this.GetExprToSubViewModel(), this.GetExprRelateToMainData(mainData));

            return new TViewGroup
            {
                MainForm = mainView,
                SubForms = subViews
            };
        }

        /// <summary>
        /// 更新副資料模型
        /// </summary>
        /// <param name="subView">副資料檢視模型</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(TSubViewModel? subView, IDataTransResult? dataTransResult, bool execNow) {
            if (subView == null) { return dataTransResult ?? this.EmptyResult; }
            return this.SubModelHelper.SaveFromView(subView, dataTransResult, execNow);
        }

        /// <summary>
        /// 更新副資料模型(複數)
        /// </summary>
        /// <param name="subViews">副資料檢視模型(複數)</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IEnumerable<TSubViewModel>? subViews, IDataTransResult? dataTransResult, bool execNow) {
            return this.SubModelHelper.SaveFromView(subViews ?? Enumerable.Empty<TSubViewModel>(), dataTransResult, execNow);
        }

        /// <summary>
        /// 從檢視資料組更新
        /// </summary>
        /// <param name="formGroup">檢視資料組</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IFormGroup<TMainViewModel, TSubViewModel>? formGroup, IDataTransResult? dataTransResult, bool execNow) {
            if (formGroup == null) { return this.MainModelHelper.EmptyResult; }
            dataTransResult = this.Update(formGroup.MainForm, dataTransResult, false);
            this.Update(formGroup.SubForms, dataTransResult, true);
            return dataTransResult;
        }

        /// <summary>
        /// 從檢視資料組更新
        /// </summary>
        /// <param name="formGroups">檢視資料組</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IEnumerable<IFormGroup<TMainViewModel, TSubViewModel>> formGroups, IDataTransResult? dataTransResult, bool execNow) {
            if (formGroups.IsNullOrEmpty()) { return this.MainModelHelper.EmptyResult; }
            dataTransResult ??= this.EmptyResult;
            int count = formGroups.Count();
            int counter = 0;
            bool should_exec = false;
            foreach (IFormGroup<TMainViewModel, TSubViewModel> formGroup in formGroups) {
                if (++counter == count && execNow) {
                    should_exec = true;
                }
                dataTransResult = this.Update(formGroup, dataTransResult, should_exec);
            }
            return dataTransResult;
        }

        protected IDataTransResult Remove(TSubViewModel? subView, IDataTransResult? dataTransResult, bool execNow) {
            return this.SubModelHelper.RemoveFromView(subView, dataTransResult, execNow);
        }

        protected IDataTransResult Remove(IEnumerable<TSubViewModel>? subViews, IDataTransResult? dataTransResult, bool execNow) {
            return this.SubModelHelper.RemoveFromView(subViews ?? Enumerable.Empty<TSubViewModel>(), dataTransResult, execNow);
        }

        protected IDataTransResult Remove(IFormGroup<TMainViewModel, TSubViewModel>? formGroup, IDataTransResult? dataTransResult, bool execNow) {
            if (formGroup == null) { return this.MainModelHelper.EmptyResult; }
            dataTransResult = this.Remove(formGroup.MainForm, dataTransResult, false);
            this.Remove(formGroup.SubForms, dataTransResult, true);
            return dataTransResult;
        }
    }

    /// <summary>
    /// 一個主資料與其關聯的多個副資料，以檢視用的模型進行存取的輔助類別
    /// </summary>
    /// <typeparam name="TMainDataModel">主資料模型</typeparam>
    /// <typeparam name="TMainViewModel">主檢視模型</typeparam>
    /// <typeparam name="IMainModel">主資料介面</typeparam>
    /// <typeparam name="TSubDataModel">副資料模型</typeparam>
    /// <typeparam name="TSubViewModel">副檢視模型</typeparam>
    /// <typeparam name="ISubModel">副資料介面</typeparam>
    public abstract class MyRelatedViewModelHelperBase<TMainDataModel, TMainViewModel, TSubDataModel, TSubViewModel, TSub2DataModel, TSub2ViewModel>
        : MyRelatedViewModelHelperBase<TMainDataModel, TMainViewModel, TSubDataModel, TSubViewModel>
        , IViewModelHelper<TSub2DataModel, TSub2ViewModel>
        , IRelatedModelHelper<TMainDataModel, TSub2DataModel>
        where TMainDataModel : class
        where TSubDataModel : class
        where TSub2DataModel : class
        where TMainViewModel : class
        where TSubViewModel : class
        where TSub2ViewModel : class
    {

        protected MyRelatedViewModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        IDataManager<TSub2DataModel>? IModelHelper<TSub2DataModel>.DefaultDataManager { get; set; }

        IViewModelHelper<TSub2DataModel, TSub2ViewModel> Sub2ModelHelper => this;

        #region 
        #endregion

        #region 介面 IViewModelHelper<TSub2DataModel, TSub2ViewModel> 的成員
        #region 由於無法被子類別存取，因此需要額外呼叫與子類別的接口方法
        Expression<Func<TSub2ViewModel, TSub2DataModel>> IViewModelHelper<TSub2DataModel, TSub2ViewModel>.GetExprToDataModel() {
            return this.GetExprToSub2DataModel();
        }

        Expression<Func<TSub2DataModel, TSub2ViewModel>> IViewModelHelper<TSub2DataModel, TSub2ViewModel>.GetExprToViewModel() {
            return this.GetExprToSub2ViewModel();
        }
        #endregion

        public abstract Expression<Func<TSub2DataModel, bool>> GetExprToFindDataModel(TSub2ViewModel view);
        public abstract void UpdateModelFromViewModel(TSub2DataModel data, TSub2ViewModel view);
        #endregion

        #region 由於介面 IViewModelHelper<TSub2DataModel, TSub2ViewModel> 的 GetExprTo... 方法無法被子類別取得，因此由此二抽象方法作為與子類別的接口
        /// <summary>
        /// 將檢視模型轉換為資料模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        public abstract Expression<Func<TSub2ViewModel, TSub2DataModel>> GetExprToSub2DataModel();

        /// <summary>
        /// 將資料模型轉換為檢視模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        public abstract Expression<Func<TSub2DataModel, TSub2ViewModel>> GetExprToSub2ViewModel();
        #endregion

        #region 介面 IRelatedModelHelper<TMainDataModel, TSub2DataModel> 的成員
        #region 由於無法被子類別存取，因此需要額外呼叫與子類別的接口方法
        Expression<Func<TSub2DataModel, bool>> IRelatedModelHelper<TMainDataModel, TSub2DataModel>.GetExprRelateToMainData(TMainDataModel data) {
            return this.GetExprRelateToMainData2(data);
        }
        #endregion
        #endregion

        #region 由於介面 IRelatedModelHelper<TSub2DataModel, TSub2ViewModel> 的 GetExprTo... 方法無法被子類別取得，因此由此抽象方法作為與子類別的接口
        /// <summary>
        /// 用來查詢與輸入主資料相關的副資料模型取得函數
        /// </summary>
        /// <param name="data">主資料模型</param>
        /// <returns>查詢用的敘述式</returns>
        public abstract Expression<Func<TSub2DataModel, bool>> GetExprRelateToMainData2(TMainDataModel data);
        #endregion

        protected override TViewGroup? GetViewGroup<TViewGroup>(Expression<Func<TMainDataModel, bool>> expression) where TViewGroup : class {
            TMainDataModel? mainData = this.MainModelHelper.GetDataModel(expression);
            if (mainData == null) { return null; }

            TViewGroup? viewGroup = base.GetViewGroup<TViewGroup>(expression);
            if (viewGroup == null) { return viewGroup; }

            if (viewGroup is IFormGroup<TMainViewModel, TSubViewModel, TSub2ViewModel> vGroup3) {
                IEnumerable<TSub2ViewModel> subViews = this.Sub2ModelHelper.GetDataModelsAs(
                    this.GetExprToSub2ViewModel(), this.GetExprRelateToMainData2(mainData));
                vGroup3.SubForms2 = subViews;
            }
            return viewGroup;
        }

        /// <summary>
        /// 更新副資料模型
        /// </summary>
        /// <param name="subView">副資料檢視模型</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(TSub2ViewModel? subView, IDataTransResult? dataTransResult, bool execNow) {
            return this.Sub2ModelHelper.SaveFromView(subView, dataTransResult, execNow);
        }

        /// <summary>
        /// 更新副資料模型(複數)
        /// </summary>
        /// <param name="subViews">副資料檢視模型(複數)</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IEnumerable<TSub2ViewModel>? subViews, IDataTransResult? dataTransResult, bool execNow) {
            return this.Sub2ModelHelper.SaveFromView(subViews ?? Enumerable.Empty<TSub2ViewModel>(), dataTransResult, execNow);
        }

        /// <summary>
        /// 從檢視資料組更新
        /// </summary>
        /// <param name="formGroup">檢視資料組</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IFormGroup<TMainViewModel, TSubViewModel, TSub2ViewModel>? formGroup, IDataTransResult? dataTransResult, bool execNow) {
            if (formGroup == null) { return this.MainModelHelper.EmptyResult; }
            dataTransResult = this.Update(formGroup.MainForm, dataTransResult, false);
            this.Update(formGroup.SubForms, dataTransResult, false);
            this.Update(formGroup.SubForms2, dataTransResult, true);
            return dataTransResult;
        }

        /// <summary>
        /// 從檢視資料組更新
        /// </summary>
        /// <param name="formGroups">檢視資料組</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IEnumerable<IFormGroup<TMainViewModel, TSubViewModel, TSub2ViewModel>> formGroups, IDataTransResult? dataTransResult, bool execNow) {
            if (formGroups.IsNullOrEmpty()) { return this.MainModelHelper.EmptyResult; }
            dataTransResult ??= this.EmptyResult;
            int count = formGroups.Count();
            int counter = 0;
            bool should_exec = false;
            foreach (IFormGroup<TMainViewModel, TSubViewModel, TSub2ViewModel> formGroup in formGroups) {
                if (++counter == count && execNow) {
                    should_exec = true;
                }
                dataTransResult = this.Update(formGroup, dataTransResult, should_exec);
            }
            return dataTransResult;
        }

        /// <summary>
        /// 更新副資料模型
        /// </summary>
        /// <param name="subView">副資料檢視模型</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Remove(TSub2ViewModel? subView, IDataTransResult? dataTransResult, bool execNow) {
            return this.Sub2ModelHelper.RemoveFromView(subView, dataTransResult, execNow);
        }

        /// <summary>
        /// 更新副資料模型(複數)
        /// </summary>
        /// <param name="subViews">副資料檢視模型(複數)</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Remove(IEnumerable<TSub2ViewModel>? subViews, IDataTransResult? dataTransResult, bool execNow) {
            return this.Sub2ModelHelper.RemoveFromView(subViews ?? Enumerable.Empty<TSub2ViewModel>(), dataTransResult, execNow);
        }
    }
}

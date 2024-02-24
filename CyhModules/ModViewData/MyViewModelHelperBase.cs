using Cyh.DataHelper;
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.Modules.ModViewData
{
    /// <summary>
    /// 資料模型以檢視用的模型進行存取的輔助類別
    /// </summary>
    /// <typeparam name="TDataModel">資料模型</typeparam>
    /// <typeparam name="TViewModel">檢視模型</typeparam>
    public abstract class MyViewModelHelperBase<TDataModel, TViewModel>
        : MyModelHelperBase<TDataModel>
        , IViewModelHelper<TDataModel, TViewModel>
        where TViewModel : class
        where TDataModel : class
    {
        static protected TDest? ConvertByExpr<TDest, TSrc>(Expression<Func<TSrc, TDest>>? expr, TSrc? src) {
            if (expr == null || src == null) return default;
            try {
                Func<TSrc, TDest> callback = expr.Compile();
                if (callback != null) { return callback(src); }
                return default;
            } catch {
                return default;
            }
        }
        protected MyViewModelHelperBase(
            IDataManagerActivator dataManagerActivator,
            IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) { }

        public IViewModelHelper<TDataModel, TViewModel> MainModelHelper => this;


        /// <summary>
        /// 將檢視模型轉換為資料模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        public abstract Expression<Func<TViewModel, TDataModel>> GetExprToDataModel();

        /// <summary>
        /// 將資料模型轉換為檢視模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        public abstract Expression<Func<TDataModel, TViewModel>> GetExprToViewModel();

        /// <summary>
        /// 用來查詢與輸入資料相關的資料模型取得函數
        /// </summary>
        /// <param name="view">檢視模型</param>
        /// <returns>查詢用的敘述式</returns>
        public abstract Expression<Func<TDataModel, bool>> GetExprToFindDataModel(TViewModel view);

        /// <summary>
        /// 用資料的檢視模型來更新資料模型
        /// </summary>
        /// <param name="data">資料模型</param>
        /// <param name="view">檢視模型</param>
        public abstract void UpdateModelFromViewModel(TDataModel data, TViewModel view);


        /// <summary>
        /// 用主資料模型的查詢式取得檢視用的模型
        /// </summary>
        /// <param name="expression">查詢敘述式</param>
        /// <returns>檢視用的模型</returns>
        protected TViewModel? GetView(Expression<Func<TDataModel, bool>>? expression) {
            return this.MainModelHelper.GetDataModelAs(this.GetExprToViewModel(), expression);
        }

        /// <summary>
        /// 用主資料模型的查詢式取得檢視用的模型
        /// </summary>
        /// <param name="expression">查詢敘述式</param>
        /// <returns>檢視用的模型</returns>
        protected IEnumerable<TViewModel> GetMultiViews(int begin, int count, Expression<Func<TDataModel, bool>>? expression) {
            return this.MainModelHelper.GetDataModelsAs(this.GetExprToViewModel(), begin, count, expression);
        }

        /// <summary>
        /// 用主要資料檢視模型來更新主要資料
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="dataTransResult"></param>
        /// <param name="execNow"></param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(TViewModel? viewModel, IDataTransResult? dataTransResult, bool execNow) {
            return this.MainModelHelper.SaveFromView(viewModel, dataTransResult, execNow);
        }

        /// <summary>
        /// 用主要資料檢視模型來更新主要資料
        /// </summary>
        /// <param name="viewModels"></param>
        /// <param name="dataTransResult"></param>
        /// <param name="execNow"></param>
        /// <returns>執行結果</returns>
        protected IDataTransResult Update(IEnumerable<TViewModel> viewModels, IDataTransResult? dataTransResult, bool execNow) {
            return this.MainModelHelper.SaveFromView(viewModels, dataTransResult, execNow);
        }

    }

    public abstract class MyViewModelHelperBase<TDataModel, TViewModel, IDataModel>
        : MyViewModelHelperBase<TDataModel, TViewModel>
        where TDataModel : class, IDataModel, new()
        where TViewModel : class, IDataModel, ISelectableModel<TViewModel, IDataModel>, new()
    {
        protected MyViewModelHelperBase(IDataManagerActivator dataManagerActivator, IDataManagerBuilder dataManagerCreaterBase)
            : base(dataManagerActivator, dataManagerCreaterBase) {
        }

        public override Expression<Func<TViewModel, TDataModel>> GetExprToDataModel() {
            return new TViewModel().GetSelectorTo<TDataModel>();
        }

        public override Expression<Func<TDataModel, TViewModel>> GetExprToViewModel() {
            return new TViewModel().GetSelectorFrom<TDataModel>();
        }

        public override void UpdateModelFromViewModel(TDataModel data, TViewModel view) {
            view.UpdateTo(data);
        }
    }
}

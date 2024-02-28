using Cyh.DataHelper;
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.Modules.ModViewData
{
    public interface IViewModelHelper : IModelHelper
    {
    }

    public interface IViewModelHelper<DataModel, ViewModel> : IModelHelper<DataModel>, IViewModelHelper where DataModel : class
    {
        /// <summary>
        /// 將資料模型轉換為檢視模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        Expression<Func<DataModel, ViewModel>> GetExprToViewModel();

        /// <summary>
        /// 將檢視模型轉換為資料模型的敘述式取得函數
        /// </summary>
        /// <returns>轉換用的敘述式</returns>
        Expression<Func<ViewModel, DataModel>> GetExprToDataModel();

        /// <summary>
        /// 用來查詢與輸入資料相關的資料模型取得函數
        /// </summary>
        /// <param name="view">副資料檢視模型</param>
        /// <returns>查詢用的敘述式</returns>
        Expression<Func<DataModel, bool>> GetExprToFindDataModel(ViewModel view);

        /// <summary>
        /// 用資料的檢視模型來更新資料模型
        /// </summary>
        /// <param name="data">資料模型</param>
        /// <param name="view">檢視模型</param>
        void UpdateModelFromViewModel(DataModel data, ViewModel view);
    }

    public static class ViewModelHelperExtends
    {
        static private TDest? ConvertByExpr<TDest, TSrc>(Expression<Func<TSrc, TDest>>? expr, TSrc? src) {
            if (expr == null || src == null) return default;
            try {
                Func<TSrc, TDest> callback = expr.Compile();
                if (callback != null) { return callback(src); }
                return default;
            } catch {
                return default;
            }
        }
        public static IDataTransResult SaveFromView<TDataModel, TViewModel>(
            this IViewModelHelper<TDataModel, TViewModel> viewModelHelper,
            TViewModel? viewModel,
            IDataTransResult? dataTransResult,
            bool execNow)
            where TDataModel : class {

            dataTransResult ??= viewModelHelper.EmptyResult;
            if (viewModel == null) { return dataTransResult; }

            TDataModel? subData = viewModelHelper.GetDataModel(viewModelHelper.GetExprToFindDataModel(viewModel));

            if (subData == null) {
                subData = ConvertByExpr(viewModelHelper.GetExprToDataModel(), viewModel);
            } else {
                viewModelHelper.UpdateModelFromViewModel(subData, viewModel);
            }
            return viewModelHelper.SaveDataModel(subData, dataTransResult, execNow);
        }

        public static IDataTransResult SaveFromView<TDataModel, TViewModel>(
            this IViewModelHelper<TDataModel, TViewModel> viewModelHelper,
            IEnumerable<TViewModel> viewModels,
            IDataTransResult? dataTransResult,
            bool execNow)
            where TDataModel : class {

            dataTransResult ??= viewModelHelper.EmptyResult;
            if (viewModels.IsNullOrEmpty()) { return dataTransResult; }

            int count = viewModels.Count();
            int counter = 0;
            bool should_exec = false;

            foreach (TViewModel subView in viewModels) {
                if (++counter == count && execNow) {
                    should_exec = true;
                }
                dataTransResult = viewModelHelper.SaveFromView(subView, dataTransResult, should_exec);
            }
            return dataTransResult;
        }

        public static IDataTransResult RemoveFromView<TDataModel, TViewModel>(
            this IViewModelHelper<TDataModel, TViewModel> viewModelHelper,
            TViewModel? viewModel,
            IDataTransResult? dataTransResult,
            bool execNow)
            where TDataModel : class {
            dataTransResult ??= viewModelHelper.EmptyResult;
            if (viewModel == null) { return dataTransResult; }
            return viewModelHelper.RemoveDataModelFrom(viewModelHelper.GetExprToDataModel(), viewModel, dataTransResult, execNow);
        }

        public static IDataTransResult RemoveFromView<TDataModel, TViewModel>(
            this IViewModelHelper<TDataModel, TViewModel> viewModelHelper,
            IEnumerable<TViewModel> viewModels,
            IDataTransResult? dataTransResult,
            bool execNow)
            where TDataModel : class {
            dataTransResult ??= viewModelHelper.EmptyResult;
            if (viewModels.IsNullOrEmpty()) { return dataTransResult; }

            int count = viewModels.Count();
            int counter = 0;
            bool should_exec = false;

            foreach (TViewModel subView in viewModels) {
                if (++counter == count && execNow) {
                    should_exec = true;
                }
                dataTransResult = viewModelHelper.RemoveDataModelFrom(viewModelHelper.GetExprToDataModel(), subView, dataTransResult, should_exec);
            }
            return dataTransResult;
        }
    }
}

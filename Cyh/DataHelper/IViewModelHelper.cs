
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 以 ViewModel 存取 DataModel 的介面
    /// </summary>
    /// <typeparam name="DataModel">DataModel</typeparam>
    /// <typeparam name="ViewModel">ViewModel</typeparam>
    public interface IViewModelHelper<DataModel, ViewModel> : IModelHelper<DataModel> where DataModel : class
    {
        /// <summary>
        /// 取得預設的主要資料管理器
        /// </summary>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        IDataManager<DataModel>? GetDefaultManager();

        Expression<Func<DataModel, ViewModel>> GetExprToViewModel();

        Expression<Func<ViewModel, DataModel>> GetExprToDataModel();
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 取得物件的 ViewModel
        /// </summary>
        /// <typeparam name="DataModel">DataModel</typeparam>
        /// <typeparam name="ViewModel">ViewModel</typeparam>
        /// <param name="expression">取得來源資料的條件</param>
        /// <param name="dataTransResult"></param>
        /// <returns>取得的ViewModel</returns>
        public static ViewModel? GetViewModel<DataModel, ViewModel>(this IViewModelHelper<DataModel, ViewModel> viewModelHelper, Expression<Func<DataModel, bool>>? expression, IDataTransResult? dataTransResult)
            where DataModel : class, new()
            where ViewModel : class, new() {
            if (viewModelHelper == null) return null;
            return viewModelHelper.GetDefaultManager().GetDataAs(viewModelHelper.GetExprToViewModel(), expression, dataTransResult);
        }

        /// <summary>
        /// 取得物件的 ViewModel
        /// </summary>
        /// <typeparam name="DataModel">DataModel</typeparam>
        /// <typeparam name="ViewModel">ViewModel</typeparam>
        /// <param name="expression">取得來源資料的條件</param>
        /// <param name="dataTransResult"></param>
        /// <returns>取得的ViewModel集合</returns>
        public static IEnumerable<ViewModel> GetViewModels<DataModel, ViewModel>(this IViewModelHelper<DataModel, ViewModel> viewModelHelper, Expression<Func<DataModel, bool>>? expression, IDataTransResult? dataTransResult)
            where DataModel : class, new()
            where ViewModel : class, new() {
            if (viewModelHelper == null) return Enumerable.Empty<ViewModel>();
            return viewModelHelper.GetDefaultManager().GetDatasAs(viewModelHelper.GetExprToViewModel(), expression, dataTransResult);
        }

        /// <summary>
        /// 取得物件的 ViewModel
        /// </summary>
        /// <typeparam name="DataModel">DataModel</typeparam>
        /// <typeparam name="ViewModel">ViewModel</typeparam>
        /// <param name="begin">開始的索引</param>
        /// <param name="count">最大取得的數量</param>
        /// <param name="expression">取得來源資料的條件</param>
        /// <param name="dataTransResult"></param>
        /// <returns>取得的ViewModel集合</returns>
        public static IEnumerable<ViewModel> GetViewModels<DataModel, ViewModel>(this IViewModelHelper<DataModel, ViewModel> viewModelHelper, int begin, int count, Expression<Func<DataModel, bool>>? expression, IDataTransResult? dataTransResult)
            where DataModel : class, new()
            where ViewModel : class, new() {
            if (viewModelHelper == null) return Enumerable.Empty<ViewModel>();
            return viewModelHelper.GetDefaultManager().GetDatasAs(viewModelHelper.GetExprToViewModel(), begin, count, expression, dataTransResult);
        }

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModel">要儲存的DataModel</param>
        /// <param name="dataTransResult">交易執行的結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult SaveFromViewModel<DataModel, ViewModel>(this IViewModelHelper<DataModel, ViewModel> viewModelHelper, ViewModel viewModel, IDataTransResult? dataTransResult, bool execNow)
            where DataModel : class, new()
            where ViewModel : class, new() {
            dataTransResult ??= new DataTransResultBase();
            if (viewModelHelper == null) return dataTransResult;
            viewModelHelper.GetDefaultManager().SaveDataFrom(viewModelHelper.GetExprToDataModel(), viewModel, dataTransResult, execNow);
            return dataTransResult;
        }

        /// <summary>
        /// 儲存DataModel(轉換自ViewModel)
        /// </summary>
        /// <param name="viewModels">要儲存的DataModel集合</param>
        /// <param name="dataTransResult">交易執行的結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult SaveFromViewModels<DataModel, ViewModel>(this IViewModelHelper<DataModel, ViewModel> viewModelHelper, IEnumerable<ViewModel> viewModels, IDataTransResult? dataTransResult, bool execNow)
            where DataModel : class, new()
            where ViewModel : class, new() {
            dataTransResult ??= new DataTransResultBase();
            if (viewModelHelper == null) return dataTransResult;
            viewModelHelper.GetDefaultManager().SaveDatasFrom(viewModelHelper.GetExprToDataModel(), viewModels, dataTransResult, execNow);
            return dataTransResult;
        }
    }
}

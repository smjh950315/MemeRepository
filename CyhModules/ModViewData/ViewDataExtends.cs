using Cyh.DataHelper;
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.Modules.ModViewData
{
    public static partial class ViewDataExtends
    {
        /// <summary>
        /// 用條件取得 ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel的類型</typeparam>
        /// <typeparam name="TDataModel">資料來源物件的類型</typeparam>
        /// <typeparam name="IModel">ViewModel與資料來源共同的介面</typeparam>
        /// <param name="expression">條件式</param>
        /// <returns>取得的ViewModel</returns>
        public static TViewModel? GetViewModel<TViewModel, TDataModel, IModel>(
            this IViewModelGetter<TDataModel, IModel> viewModelGetter,
            Expression<Func<TDataModel, bool>>? expression
            )
            where TViewModel : class, IModel, ISelectableDestModel<TViewModel, IModel>, new()
            where TDataModel : IModel {

            if (viewModelGetter == null)
                return default;

            TViewModel t = new();

            return viewModelGetter.GetDataSource().TryGetDataAs(t.GetSelectorFrom<TDataModel>(), expression);
        }

        /// <summary>
        /// 用條件取得所有符合條件的 ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel的類型</typeparam>
        /// <typeparam name="TDataModel">資料來源物件的類型</typeparam>
        /// <typeparam name="IModel">ViewModel與資料來源共同的介面</typeparam>
        /// <param name="expression">條件式</param>
        /// <returns>取得的ViewModel集合</returns>
        public static IEnumerable<TViewModel> GetViewModels<TViewModel, TDataModel, IModel>(
            this IViewModelGetter<TDataModel, IModel> viewModelGetter,
            Expression<Func<TDataModel, bool>>? expression
            )
            where TViewModel : class, IModel, ISelectableDestModel<TViewModel, IModel>, new()
            where TDataModel : IModel {

            if (viewModelGetter == null)
                return Enumerable.Empty<TViewModel>();

            TViewModel t = new();

            return viewModelGetter.GetDataSource().TryGetDatasAs(t.GetSelectorFrom<TDataModel>(), expression) ?? Enumerable.Empty<TViewModel>();
        }
    }
}

using Cyh.DataHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.Modules.ModViewData
{
    public static partial class ViewDataExtends
    {
        public static TViewModel? GetViewModel<TViewModel, TDataModel, IModel>(
            this IViewModelGetter<TViewModel, TDataModel, IModel> viewModelGetter,
            Expression<Func<TDataModel, bool>>? expression
            )
            where TViewModel : IModel, ISelectableDataModel<TViewModel, IModel>, new()
            where TDataModel : IModel {

            if (viewModelGetter == null)
                return default;

            TViewModel t = new();

            return viewModelGetter.DataAccesser.TryGetDataAs(t.GetSelector<TDataModel>(), expression);
        }

        public static IEnumerable<TViewModel> GetViewModels<TViewModel, TDataModel, IModel>(
            this IViewModelGetter<TViewModel, TDataModel, IModel> viewModelGetter,
            Expression<Func<TDataModel, bool>>? expression
            )
            where TViewModel : IModel, ISelectableDataModel<TViewModel, IModel>, new()
            where TDataModel : IModel {

            if (viewModelGetter == null)
                return Enumerable.Empty<TViewModel>();

            TViewModel t = new();

            return viewModelGetter.DataAccesser.TryGetDatasAs(t.GetSelector<TDataModel>(), expression) ?? Enumerable.Empty<TViewModel>();
        }
    }
}

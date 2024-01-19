using Cyh.DataHelper;
using Cyh.Modules.ModForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Cyh.WebServices.Controller
{
    public static partial class MyControllerExtends
    {

        public static TViewModel? GetViewModel<TViewModel, TDataModel, IModel>(
            this IViewModelController<TViewModel, TDataModel, IModel> viewModelController,
            Expression<Func<TDataModel, bool>>? expression
            )
            where TViewModel : IModel, ISelectableDataModel<TViewModel, IModel>, new()
            where TDataModel : IModel {

            if (viewModelController == null)
                return default;

            TViewModel t = new();

            return viewModelController.FormManager.GetMainFormAs(t.GetSelector<TDataModel>(), expression);
        }

        public static IEnumerable<TViewModel> GetViewModels<TViewModel, TDataModel, IModel>(
            this IViewModelController<TViewModel, TDataModel, IModel> viewModelController,
            Expression<Func<TDataModel, bool>>? expression
            )
            where TViewModel : IModel, ISelectableDataModel<TViewModel, IModel>, new()
            where TDataModel : IModel {

            if (viewModelController == null)
                return Enumerable.Empty<TViewModel>();

            TViewModel t = new();

            return viewModelController.FormManager.GetMainFormsAs(t.GetSelector<TDataModel>(), expression) ?? Enumerable.Empty<TViewModel>();
        }

    }
}

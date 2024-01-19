using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    public interface ISelectableDataModel<TViewData, IViewData>
    {
        Expression<Func<TData, TViewData>> GetSelector<TData>() where TData : IViewData;
    }
}

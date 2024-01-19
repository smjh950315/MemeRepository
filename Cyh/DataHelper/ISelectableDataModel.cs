using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    public interface ISelectableDataModel<TViewData, IViewData>
    {
        /// <summary>
        /// 取得選擇器
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns>選擇器</returns>
        Expression<Func<TData, TViewData>> GetSelector<TData>() where TData : IViewData;
    }
}

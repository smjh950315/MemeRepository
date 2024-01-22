using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 可以將擁有相同介面屬性的來源資料模型轉換成當前類型的介面
    /// </summary>
    /// <typeparam name="TViewData">當前模型的類別</typeparam>
    /// <typeparam name="IViewData">當前模型要擷取資料的介面屬性</typeparam>
    public interface ISelectableDataModel<TViewData, IViewData>
    {
        /// <summary>
        /// 將輸入的資料模型轉換成當前類型的選擇器
        /// </summary>
        /// <typeparam name="TData">來源資料模型，必須要與當前模型有相同介面屬性</typeparam>
        /// <returns>轉換用的選擇器</returns>
        Expression<Func<TData, TViewData>> GetSelector<TData>() where TData : IViewData;
    }
}

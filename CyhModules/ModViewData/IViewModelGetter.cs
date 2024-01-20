using Cyh.DataHelper;

namespace Cyh.Modules.ModViewData
{
    /// <summary>
    /// 讓輸入輸出類型可以透過相同介面轉換的 ViewModel 取得器
    /// </summary>
    /// <typeparam name="TDataModel">資料來源，需要實作輸入介面</typeparam>
    /// <typeparam name="IViewModel">輸入與輸出資料共同的介面</typeparam>
    public interface IViewModelGetter<TDataModel, IViewModel>
        where TDataModel : IViewModel
    {
        /// <summary>
        /// 取得來源資料存取器
        /// </summary>
        /// <returns>來源資料存取器</returns>
        IMyDataAccesser<TDataModel> GetDataSource();
    }
}

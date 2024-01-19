using Cyh.DataHelper;

namespace Cyh.Modules.ModPager
{
    /// <summary>
    /// 僅供讀取的資料分頁清單介面
    /// </summary>
    public interface IReadOnlyPageList
    {
        /// <summary>
        /// 頁數
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 每一頁能容納的內容數量
        /// </summary>
        int PageCapacity { get; }

        /// <summary>
        /// 取得指定頁的資料
        /// </summary>
        /// <param name="page_count"></param>
        /// <returns>取得僅供讀取的頁面資料</returns>
        IReadOnlyPage GetPage(int page_count);
    }

    /// <summary>
    /// 資料的分頁清單介面
    /// </summary>
    public interface IPageList : IReadOnlyPageList
    {
        /// <summary>
        /// 設定要取得的資料來源
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>設定是否成功</returns>
        bool SetDataSource(IReadOnlyDataAccesser dataReader);

        /// <summary>
        /// 取得空的頁面用來儲存資料
        /// </summary>
        /// <returns>新建的頁面</returns>
        IPage CreateNewPage();
    }

}

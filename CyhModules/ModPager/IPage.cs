using System.Collections;

namespace Cyh.Modules.ModPager
{
    /// <summary>
    /// 僅供讀取的頁面資料介面
    /// </summary>
    public interface IReadOnlyPage : IEnumerable
    {
        /// <summary>
        /// 儲存的類型資訊
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// 當前的頁碼
        /// </summary>
        int Index { get; }

        /// <summary>
        /// 本頁的物件數
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 當前頁的容量
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// 取得指定索引的資料
        /// </summary>
        /// <param name="index"></param>
        /// <returns>如果索引超過保存的資料數，回傳 null</returns>
        object? GetItem(int index);
    }

    /// <summary>
    /// 頁面資料介面
    /// </summary>
    public interface IPage : IReadOnlyPage
    {
        /// <summary>
        /// 設定頁面的保存資料
        /// </summary>
        /// <param name="items">要匯入頁面的資料</param>
        void SetPageItems(IEnumerable items);
    }
}

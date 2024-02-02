using Cyh.Common;

namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 資料驗證器
    /// </summary>
    public interface IMyDataValidator
    {
        /// <summary>
        /// 驗證資料的Key字典，紀錄當前可以接受的key有哪些
        /// </summary>
        IEnumerable<string> ValidationKeys { get; }

        /// <summary>
        /// 輸入的驗證資料集是否有效
        /// </summary>
        /// <param name="myDataSet">驗證用的資料集</param>
        /// <returns>是否通過驗證</returns>
        bool IsValid(MyDataSet? myDataSet);
    }
}

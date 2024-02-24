using Cyh.Common;

namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 認證器
    /// </summary>
    public interface IClientAuthorizer
    {
        /// <summary>
        /// 驗證資料的Key字典，紀錄當前可以接受的key有哪些
        /// </summary>
        IEnumerable<string> ValidationKeys { get; }

        /// <summary>
        /// 認證輸入資料是否合法
        /// </summary>
        /// <param name="myDataSet">認證用的資料集</param>
        /// <returns>是否有符合的客戶端資料</returns>
        bool IsValid(MyDataSet? myDataSet);

        /// <summary>
        /// 取得輸入認證資料集對應的Key(或ID)
        /// </summary>
        /// <param name="myDataSet">認證用的資料集</param>
        /// <returns>資料集對應的客戶端Key，如果輸入的認證不存在，回傳null</returns>
        string? GetExistKey(MyDataSet? myDataSet);
    }
}

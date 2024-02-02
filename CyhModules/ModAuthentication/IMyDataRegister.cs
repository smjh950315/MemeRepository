using Cyh.Common;

namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 授權資料的註冊器
    /// </summary>
    public interface IMyDataRegister
    {
        /// <summary>
        /// 授權資料的Key字典，紀錄當前可以接受的key有哪些
        /// </summary>
        IEnumerable<string> AcceptableKeys { get; }

        /// <summary>
        /// 註冊授權資料
        /// </summary>
        /// <param name="myDataSet">註冊授權的資料</param>
        /// <returns>是否成功註冊</returns>
        bool Register(MyDataSet? myDataSet);
    }
}

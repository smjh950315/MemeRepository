namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料交易後的結果
    /// </summary>
    public interface IDataTransResult
    {
        /// <summary>
        /// 資料交易的總筆量
        /// </summary>
        int TotalTransCount { get; set; }

        /// <summary>
        /// 成功交易的筆數
        /// </summary>
        int SucceedTransCount { get; set; }

        /// <summary>
        /// 交易的訊息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 交易的執行者
        /// </summary>
        string Accesser { get; set; }

        /// <summary>
        /// 交易開始的時間
        /// </summary>
        DateTime BeginTime { get; set; }

        /// <summary>
        /// 交易結束的時間
        /// </summary>
        DateTime EndTime { get; set; }

        /// <summary>
        /// 每筆交易的紀錄
        /// </summary>
        TransDetails[] TransDetails { get; set; }
    }
}

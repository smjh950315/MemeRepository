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
        /// 交易開始的時間
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 交易結束的時間
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 每筆交易的紀錄
        /// </summary>
        public TransDetails[] TransDetails { get; set; }
    }
}

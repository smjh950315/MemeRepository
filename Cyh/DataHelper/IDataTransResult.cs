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
    }
}

using Cyh.DataHelper;

namespace Cyh.EFCore
{
    /// <summary>
    /// DB 交易的結果
    /// </summary>
    public class DbTransResult : IDataTransResult
    {
        /// <summary>
        /// 交易的總數
        /// </summary>
        public int TotalTransCount { get; set; }

        /// <summary>
        /// 交易成功數
        /// </summary>
        public int SucceedTransCount { get; set; }
    }
}

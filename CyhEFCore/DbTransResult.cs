using Cyh.DataHelper;

namespace Cyh.EFCore
{
    /// <summary>
    /// DB 交易的結果
    /// </summary>
    public class DbTransResult : IDataTransResult
    {
        TransDetails[] _TransDetails;

        /// <summary>
        /// 交易的總數
        /// </summary>
        public int TotalTransCount { get; set; }

        /// <summary>
        /// 交易成功數
        /// </summary>
        public int SucceedTransCount { get; set; }

        /// <summary>
        /// 交易開始的時間
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 交易結束的時間
        /// </summary>
        public DateTime EndTime { get; set; }

        public TransDetails[] TransDetails {
            get {
                if (this._TransDetails == null)
                    this._TransDetails = new TransDetails[this.TotalTransCount];
                return this._TransDetails;
            }
            set { this._TransDetails = value; }
        }
    }
}

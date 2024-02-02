namespace Cyh.DataModels
{
    /// <summary>
    /// 每筆交易的紀錄
    /// </summary>
    public class TransDetails
    {
        /// <summary>
        /// 在資料集合中的索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 額外訊息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 加入貯列的時間
        /// </summary>
        public DateTime InqueueTime { get; set; }

        /// <summary>
        /// 執行交易的時間
        /// </summary>
        public DateTime InvokedTime { get; set; }

        public TransDetails() {
            this.Index = -1;
            this.IsSucceed = false;
            this.Message = null;
            this.InqueueTime = DateTime.Now;
            this.InvokedTime = DateTime.Now;
        }

        public TransDetails(bool _isSucceed, string? message = null) {
            this.Index = 0;
            this.IsSucceed = _isSucceed;
            this.Message = message;
            this.InqueueTime = DateTime.Now;
            this.InvokedTime = DateTime.Now;
        }

        public TransDetails(int index_of_data, bool _isSucceed, string? message = null) {
            this.Index = index_of_data;
            this.IsSucceed = _isSucceed;
            this.Message = message;
            this.InqueueTime = DateTime.Now;
            this.InvokedTime = DateTime.Now;
        }
    }
}

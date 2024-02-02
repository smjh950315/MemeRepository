namespace Cyh.DataModels
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
        List<TransDetails> TransDetails { get; set; }

        /// <summary>
        /// 標記結束，不再使用
        /// </summary>
        bool IsFinished { get; set; }
    }

    public static class DataTransResultExtends
    {
        /// <summary>
        /// 加入一次執行的結果
        /// </summary>
        /// <param name="_isSucceed">是否成功，如果是批量執行，則此選項只會按照最終結果做設定</param>
        /// <param name="message">執行訊息</param>
        /// <exception cref="Exception">如果結果已經設定，會拋出不可再次設定的例外</exception>
        public static void TryAppendTransResult(this IDataTransResult? dataTransResult, bool _isSucceed, string? message = null) {
            if (dataTransResult != null) {
                if (dataTransResult.IsFinished) {
                    throw new Exception("該批次資料交換已經結束，不得再修改");
                }
                dataTransResult.TransDetails.Add(new TransDetails(dataTransResult.TransDetails.Count, _isSucceed, message));
                dataTransResult.TotalTransCount += 1;
                if (_isSucceed)
                    dataTransResult.SucceedTransCount += 1;
            }
        }

        /// <summary>
        /// 設定批量執行後的結果
        /// </summary>
        /// <param name="totalResult">批量執行後的結果</param>
        /// <exception cref="Exception">如果結果已經設定，會拋出不可再次設定的例外</exception>
        public static void BatchOnFinish(this IDataTransResult? dataTransResult, bool totalResult) {
            if (dataTransResult != null) {
                if (dataTransResult.IsFinished) {
                    return;
                }
                foreach (TransDetails item in dataTransResult.TransDetails) {
                    item.IsSucceed = totalResult;
                    item.InvokedTime = DateTime.Now;
                }
                if (totalResult) {
                    dataTransResult.SucceedTransCount = 0;
                }
                dataTransResult.IsFinished = true;
            }
        }
    }
}

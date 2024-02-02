using Cyh.DataModels;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料寫入器
    /// </summary>
    public interface IWritableDataAccesser
    {
        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        bool IsAccessable { get; }

        /// <summary>
        /// 存取的資料型別
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// 空的結果
        /// </summary>
        IDataTransResult EmptyResult { get; }

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInput">要寫入資料</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行成功與否，如不選擇立即執行，則回傳 TRUE 表示已加入貯列</returns>
        bool TryAddOrUpdateSingle(object? dataInput, IDataTransResult? prevResult = null, bool execNow = true);

        /// <summary>
        /// 處理例外
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception? exception);
    }

    /// <summary>
    /// 資料寫入器
    /// </summary>
    /// <typeparam name="T">要寫入的資料型別</typeparam>
    public interface IWritableDataAccesser<T> : IWritableDataAccesser
    {
        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="data">要寫入的資料</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>成功與否，如不選擇立即執行，則回傳 TRUE 表示已加入貯列</returns>
        bool TryAddOrUpdate(T data, IDataTransResult? prevResult = null, bool execNow = true);

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInput">要寫入的資料清單</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果，如不選擇立即執行，則回傳 TRUE 表示已加入貯列</returns>
        IDataTransResult TryAddOrUpdate(IEnumerable<T> dataInput, IDataTransResult? prevResult = null, bool execNow = true);
    }
}

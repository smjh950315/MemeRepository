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
        /// <returns>執行成功與否</returns>
        bool TryAddOrUpdateSingle(object? dataInput);

        /// <summary>
        /// 處理例外的方法
        /// </summary>
        /// <param name="exception"></param>
        void ExceptionHandler(Exception? exception);
    }

    /// <summary>
    /// 資料寫入器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWritableDataAccesser<T> : IWritableDataAccesser
    {
        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="data">要寫入的資料</param>
        /// <returns>成功與否</returns>
        bool TryAddOrUpdate(T data);

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInput">要寫入的資料清單</param>
        /// <returns>執行結果</returns>
        IDataTransResult TryAddOrUpdate(IEnumerable<T> dataInput);
    }
}

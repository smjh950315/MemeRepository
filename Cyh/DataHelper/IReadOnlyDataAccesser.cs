namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料讀取器
    /// </summary>
    public interface IReadOnlyDataAccesser
    {
        /// <summary>
        /// 存取的資料型別
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        bool IsAccessable { get; }

        /// <summary>
        /// 處理例外
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception? exception);
    }

    /// <summary>
    /// 資料讀取器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyDataAccesser<T> : IReadOnlyDataAccesser
    {
        /// <summary>
        /// 資料源查詢的介面
        /// </summary>
        IQueryable<T>? Queryable { get; }
    }
}

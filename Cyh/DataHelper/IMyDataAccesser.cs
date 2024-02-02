namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料存取器
    /// </summary>
    public interface IMyDataAccesser : IWritableDataAccesser, IReadOnlyDataAccesser
    {
#pragma warning disable CS0108
        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        bool IsAccessable { get; }

        /// <summary>
        /// 處理例外
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception? exception);
#pragma warning restore CS0108

        /// <summary>
        /// 設定存取者 ID
        /// </summary>
        /// <param name="accesserId">存取者 ID</param>
        void SetAccesserId(string accesserId);
    }


    /// <summary>
    /// 資料存取器
    /// </summary>
    public interface IMyDataAccesser<T> : IMyDataAccesser, IWritableDataAccesser<T>, IReadOnlyDataAccesser<T>
    {
    }
}

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料存取器
    /// </summary>
    public interface IMyDataAccesser : IWritableDataAccesser, IReadOnlyDataAccesser
    {

    }


    /// <summary>
    /// 資料存取器
    /// </summary>
    public interface IMyDataAccesser<T> : IMyDataAccesser, IWritableDataAccesser<T>, IReadOnlyDataAccesser<T>
    {
#pragma warning disable CS0108
        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        bool IsAccessable { get; }
#pragma warning restore CS0108
    }
}

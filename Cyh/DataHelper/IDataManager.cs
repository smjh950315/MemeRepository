namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器的介面，資料源需要經過設定才能使用
    /// </summary>
    public interface IDataManager
    {
    }

    /// <summary>
    /// 資料管理器的介面，資料源需要經過設定才能使用
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    public interface IDataManager<T> : IDataManager
    {
        /// <summary>
        /// 主要資料來源
        /// </summary>
        IMyDataAccesser<T>? MainDataSource { get; set; }
    }

    /// <summary>
    /// 資料管理器的介面，資料源需要經過設定才能使用
    /// </summary>
    /// <typeparam name="T">主要資料的模型</typeparam>
    /// <typeparam name="U">次要資料的模型</typeparam>
    public interface IDataManager<T, U> : IDataManager<T>
    {
        /// <summary>
        /// 次要資料的來源
        /// </summary>
        IMyDataAccesser<U>? SubDataSource { get; set; }
    }

    /// <summary>
    /// 資料管理器的介面，資料源需要經過設定才能使用
    /// </summary>
    /// <typeparam name="T">主要資料的模型</typeparam>
    /// <typeparam name="U">次要資料的模型1</typeparam>
    /// <typeparam name="V">次要資料的模型2</typeparam>
    public interface IDataManager<T, U, V> : IDataManager<T, U>
    {
        /// <summary>
        /// 次要資料2的來源
        /// </summary>
        IMyDataAccesser<V>? SubDataSource2 { get; set; }
    }
}

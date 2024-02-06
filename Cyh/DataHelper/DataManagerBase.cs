namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    public class DataManagerBase : IDataManager
    {
    }

    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    public class DataManagerBase<T> : DataManagerBase, IDataManager<T>
    {
        public IMyDataAccesser<T>? MainDataSource { get; set; }
    }

    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    /// <typeparam name="T">主要資料的模型</typeparam>
    /// <typeparam name="U">次要資料的模型</typeparam>
    public class DataManagerBase<T, U> : DataManagerBase<T>, IDataManager<T, U>
    {
        public IMyDataAccesser<U>? SubDataSource { get; set; }
    }

    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    /// <typeparam name="T">主要資料的模型</typeparam>
    /// <typeparam name="U">次要資料的模型1</typeparam>
    /// <typeparam name="V">次要資料的模型2</typeparam>
    public class DataManagerBase<T, U, V> : DataManagerBase<T, U>, IDataManager<T, U, V>
    {
        public IMyDataAccesser<V>? SubDataSource2 { get; set; }
    }
}

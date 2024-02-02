namespace Cyh.DataHelper
{
    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    public interface IDataManager
    {
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IDataManager? GetDefault();
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IDataManager<T>? GetDefault<T>();
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IDataManager<T, U>? GetDefault<T, U>();
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IDataManager<T, U, V>? GetDefault<T, U, V>();
    }

    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    /// <typeparam name="MFEntity">表單的模型</typeparam>
    public interface IDataManager<TForm> : IDataManager
    {
        /// <summary>
        /// 表單的資料源
        /// </summary>
        IMyDataAccesser<TForm>? MainDataSource { get; set; }
    }

    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity">表身的模型</typeparam>
    public interface IDataManager<MFEntity, TFEntity> : IDataManager<MFEntity>
    {
        /// <summary>
        /// 表身的資料源
        /// </summary>
        IMyDataAccesser<TFEntity>? SubDataSource { get; set; }
    }

    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity1">表身的模型1</typeparam>
    /// <typeparam name="TFEntity2">表身的模型2</typeparam>
    public interface IDataManager<MFEntity, TFEntity1, TFEntity2> : IDataManager<MFEntity, TFEntity1>
    {
        /// <summary>
        /// 第二表身的資料源
        /// </summary>
        IMyDataAccesser<TFEntity2>? SubDataSource2 { get; set; }
    }
}

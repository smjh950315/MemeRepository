namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器活性化工具的介面
    /// </summary>
    public interface IDataManagerActivator
    {
        /// <summary>
        /// 取得資料存取器
        /// </summary>
        /// <typeparam name="T">要存取的資料型別</typeparam>
        /// <returns>失敗時回傳 null</returns>
        IMyDataAccesser<T>? TryGetDataAccesser<T>() where T : class;
    }

    /// <summary>
    /// 資料管理活性化工具的介面
    /// </summary>
    public interface IDataManagerCreater
    {
        /// <summary>
        /// 新建未初始化的資料管理器
        /// </summary>
        /// <typeparam name="TDataManager">資料管理器</typeparam>
        /// <returns>未初始化得資料管理器</returns>
        TDataManager CreateManager<TDataManager>() where TDataManager : IDataManager;
    }
}

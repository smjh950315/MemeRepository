namespace Cyh.DataHelper
{
    /// <summary>
    /// 表單管理活性化工具的介面
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
    /// 表單管理活性化工具的介面
    /// </summary>
    public interface IDataManagerCreater
    {
        /// <summary>
        /// 新建未初始化的表單管理器
        /// </summary>
        /// <typeparam name="TFormManager">表單管理器</typeparam>
        /// <returns>未初始化得表單管理器</returns>
        TFormManager CreateManager<TFormManager>() where TFormManager : IDataManager;
    }
}

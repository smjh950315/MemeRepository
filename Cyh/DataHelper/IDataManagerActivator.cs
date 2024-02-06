namespace Cyh.DataHelper
{
    /// <summary>
    /// 為資料管理器設定資料來源的物件
    /// </summary>
    public interface IDataManagerActivator
    {
        /// <summary>
        /// 取得資料存取器 (會用來設定資料管理器的資料來源)
        /// </summary>
        /// <typeparam name="T">要存取的資料型別</typeparam>
        /// <returns>失敗時回傳 null</returns>
        IMyDataAccesser<T>? TryGetDataAccesser<T>() where T : class;
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 活性化資料管理器
        /// </summary>
        /// <typeparam name="T">資料的模型</typeparam>
        /// <param name="newMgr">資料管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T>(this IDataManagerActivator? activator, IDataManager<T>? newMgr)
            where T : class {

            if (activator == null || newMgr == null)
                return false;

            newMgr.MainDataSource = activator.TryGetDataAccesser<T>();

            return newMgr.MainDataSource != null;
        }

        /// <summary>
        /// 活性化資料管理器
        /// </summary>
        /// <typeparam name="T">資料單頭的模型</typeparam>
        /// <typeparam name="U">資料單身的模型</typeparam>
        /// <param name="newMgr">資料管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T, U>(this IDataManagerActivator? activator, IDataManager<T, U>? newMgr)
            where T : class
            where U : class {

            if (activator == null || newMgr == null)
                return false;

            newMgr.MainDataSource = activator.TryGetDataAccesser<T>();
            newMgr.SubDataSource = activator.TryGetDataAccesser<U>();

            return newMgr.MainDataSource != null;
        }

        /// <summary>
        /// 活性化資料管理器
        /// </summary>
        /// <typeparam name="T">資料單頭的模型</typeparam>
        /// <typeparam name="U">資料單身的模型1</typeparam>
        /// <typeparam name="V">資料單身的模型2</typeparam>
        /// <param name="newMgr">資料管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T, U, V>(this IDataManagerActivator? formManager, IDataManager<T, U, V>? newMgr)
            where T : class
            where U : class
            where V : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainDataSource = formManager.TryGetDataAccesser<T>();
            newMgr.SubDataSource = formManager.TryGetDataAccesser<U>();
            newMgr.SubDataSource2 = formManager.TryGetDataAccesser<V>();

            return newMgr.MainDataSource != null;
        }
    }
}

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器(未初始化)的產生器介面
    /// </summary>
    public interface IDataManagerBuilder
    {
        /// <summary>
        /// 取得預設資料管理器(未初始化)
        /// </summary>
        /// <returns>資料管理器(未初始化)</returns>
        IDataManager? GetDefault();

        /// <summary>
        /// 取得預設資料管理器(未初始化)
        /// </summary>
        /// <typeparam name="T">資料的模型</typeparam>
        /// <returns>資料管理器(未初始化)</returns>
        IDataManager<T>? GetDefault<T>();

        /// <summary>
        /// 取得預設資料管理器(未初始化)
        /// </summary>
        /// <typeparam name="T">資料的模型</typeparam>
        /// <typeparam name="U">資料的模型2</typeparam>
        /// <returns>資料管理器(未初始化)</returns>
        IDataManager<T, U>? GetDefault<T, U>();

        /// <summary>
        /// 取得預設資料管理器(未初始化)
        /// </summary>
        /// <typeparam name="T">資料的模型</typeparam>
        /// <typeparam name="U">資料的模型2</typeparam>
        /// <typeparam name="V">資料的模型3</typeparam>
        /// <returns>資料管理器(未初始化)</returns>
        IDataManager<T, U, V>? GetDefault<T, U, V>();
    }

    /// <summary>
    /// 建立資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    public interface IDataManagerBuilder<T> : IDataManagerBuilder { }

    /// <summary>
    /// 建立資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    /// <typeparam name="U">資料的模型2</typeparam>
    public interface IDataManagerBuilder<T, U> : IDataManagerBuilder<T> { }

    /// <summary>
    /// 建立資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    /// <typeparam name="U">資料的模型2</typeparam>
    /// <typeparam name="V">資料的模型3</typeparam>
    public interface IDataManagerBuilder<T, U, V> : IDataManagerBuilder<T, U> { }

}

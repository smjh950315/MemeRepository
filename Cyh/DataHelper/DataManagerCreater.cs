namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器(未初始化)的產生器介面
    /// </summary>
    public interface IDataManagerCreater
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
    public interface IDataManagerCreater<T> : IDataManagerCreater { }

    /// <summary>
    /// 建立資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    /// <typeparam name="U">資料的模型2</typeparam>
    public interface IDataManagerCreater<T, U> : IDataManagerCreater<T> { }

    /// <summary>
    /// 建立資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    /// <typeparam name="U">資料的模型2</typeparam>
    /// <typeparam name="V">資料的模型3</typeparam>
    public interface IDataManagerCreater<T, U, V> : IDataManagerCreater<T, U> { }

    /// <summary>
    /// 資料管理器(未初始化)的產生器
    /// </summary>
    public class DataManagerCreater : IDataManagerCreater
    {
        public virtual IDataManager? GetDefault() {
            throw new NotImplementedException();
        }

        public virtual IDataManager<T>? GetDefault<T>() {
            return new DataManagerBase<T>();
        }

        public virtual IDataManager<T, U>? GetDefault<T, U>() {
            return new DataManagerBase<T, U>();
        }

        public virtual IDataManager<T, U, V>? GetDefault<T, U, V>() {
            return new DataManagerBase<T, U, V>();
        }
    }

    /// <summary>
    /// 資料管理器(未初始化)的產生器
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    public class DataManagerCreater<T> : DataManagerCreater, IDataManagerCreater<T>
    {
        public override IDataManager? GetDefault() {
            return new DataManagerBase<T>();
        }
    }

    /// <summary>
    /// 資料管理器(未初始化)的產生器
    /// </summary>
    /// <typeparam name="T">資料的模型1</typeparam>
    /// <typeparam name="U">資料的模型2</typeparam>
    public class DataManagerCreater<T, U> : DataManagerCreater<T>, IDataManagerCreater<T, U>
    {
        public override IDataManager? GetDefault() {
            return new DataManagerBase<T, U>();
        }
    }

    /// <summary>
    /// 資料管理器(未初始化)的產生器
    /// </summary>
    /// <typeparam name="T">資料的模型1</typeparam>
    /// <typeparam name="U">資料的模型2</typeparam>
    /// <typeparam name="V">資料的模型3</typeparam>
    public class DataManagerCreater<T, U, V> : DataManagerCreater<T, U>, IDataManagerCreater<T, U, V>
    {
        public override IDataManager? GetDefault() {
            return new DataManagerBase<T, U, V>();
        }
    }
}

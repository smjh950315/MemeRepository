namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器(未初始化)的產生器
    /// </summary>
    public class MyDataManagerBuilderBase : IDataManagerBuilder
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
    public class MyDataManagerBuilderBase<T> : MyDataManagerBuilderBase, IDataManagerBuilder<T>
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
    public class MyDataManagerBuilderBase<T, U> : MyDataManagerBuilderBase<T>, IDataManagerBuilder<T, U>
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
    public class MyDataManagerBuilderBase<T, U, V> : MyDataManagerBuilderBase<T, U>, IDataManagerBuilder<T, U, V>
    {
        public override IDataManager? GetDefault() {
            return new DataManagerBase<T, U, V>();
        }
    }
}

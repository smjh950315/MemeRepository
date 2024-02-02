namespace Cyh.DataHelper
{
    /// <summary>
    /// 集合資料管理器的實體
    /// </summary>
    public class DataManagerBase : IDataManager
    {
        public virtual IDataManager? GetDefault() {
            throw new NotImplementedException();
        }
        public virtual IDataManager<T>? GetDefault<T>() {
            throw new NotImplementedException();
        }
        public virtual IDataManager<T, U>? GetDefault<T, U>() {
            throw new NotImplementedException();
        }
        public virtual IDataManager<T, U, V>? GetDefault<T, U, V>() {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 集合資料管理器的實體
    /// </summary>
    /// <typeparam name="MFEntity">表單的模型</typeparam>
    public class DataManagerBase<MFEntity> : DataManagerBase, IDataManager<MFEntity>
    {
        public IMyDataAccesser<MFEntity>? MainDataSource { get; set; }
        public override IDataManager? GetDefault() {
            return new DataManagerBase<MFEntity>();
        }
    }

    /// <summary>
    /// 集合資料管理器的實體
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity">表身的模型</typeparam>
    public class DataManagerBase<MFEntity, TFEntity> : DataManagerBase<MFEntity>, IDataManager<MFEntity, TFEntity>
    {
        public IMyDataAccesser<TFEntity>? SubDataSource { get; set; }
        public override IDataManager? GetDefault() {
            return new DataManagerBase<MFEntity, TFEntity>();
        }
    }

    /// <summary>
    /// 集合資料管理器的實體
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity1">表身的模型1</typeparam>
    /// <typeparam name="TFEntity2">表身的模型2</typeparam>
    public class DataManagerBase<MFEntity, TFEntity1, TFEntity2> : DataManagerBase<MFEntity, TFEntity1>, IDataManager<MFEntity, TFEntity1, TFEntity2>
    {
        public IMyDataAccesser<TFEntity2>? SubDataSource2 { get; set; }
        public override IDataManager? GetDefault() {
            return new DataManagerBase<MFEntity, TFEntity1, TFEntity2>();
        }
    }
}

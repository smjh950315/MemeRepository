using Cyh.DataHelper;

namespace Cyh.Modules.ModForm
{
    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    public interface IFormManager
    {
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IFormManager? GetDefault();
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IFormManager<T>? GetDefault<T>();
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IFormManager<T, U>? GetDefault<T, U>();
        /// <summary>
        /// 取得預設表單管理器
        /// </summary>
        /// <returns>未初始化的表單管理器</returns>
        IFormManager<T, U, V>? GetDefault<T, U, V>();
    }

    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    /// <typeparam name="MFEntity">表單的模型</typeparam>
    public interface IFormManager<MFEntity> : IFormManager
    {
        /// <summary>
        /// 表單的資料源
        /// </summary>
        IMyDataAccesser<MFEntity>? MainFormSource { get; set; }
    }

    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity">表身的模型</typeparam>
    public interface IFormManager<MFEntity, TFEntity> : IFormManager<MFEntity>
    {
        /// <summary>
        /// 表身的資料源
        /// </summary>
        IMyDataAccesser<TFEntity>? TargetFormSource { get; set; }
    }

    /// <summary>
    /// 表單管理器的介面
    /// </summary>
    /// <typeparam name="MFEntity">表頭的模型</typeparam>
    /// <typeparam name="TFEntity1">表身的模型1</typeparam>
    /// <typeparam name="TFEntity2">表身的模型2</typeparam>
    public interface IFormManager<MFEntity, TFEntity1, TFEntity2> : IFormManager<MFEntity, TFEntity1>
    {
        /// <summary>
        /// 第二表身的資料源
        /// </summary>
        IMyDataAccesser<TFEntity2>? TargetFormSource2 { get; set; }
    }


}

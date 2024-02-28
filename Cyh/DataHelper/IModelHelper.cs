using Cyh.DataModels;

namespace Cyh.DataHelper
{
    /// <summary>
    /// <para>物件模型存取工具的基底介面，儲存用來設定資料源的 DataManagerActivator 以及建立資料管理器的 DataManagerBuilder</para>
    /// <para>可以在需要時才建立並活化資料管理器</para>
    /// </summary>
    public interface IModelHelper
    {
        /// <summary>
        /// 資料管理器設定器
        /// </summary>
        IDataManagerActivator DataManagerActivator { get; }

        /// <summary>
        /// 資料管理器(未初始化)產生器
        /// </summary>
        IDataManagerBuilder DataManagerBuilder { get; }

        /// <summary>
        /// 空的資料交換結果
        /// </summary>
        IDataTransResult EmptyResult { get; }

        /// <summary>
        /// 主要的模型資訊
        /// </summary>
        Type? ModelType { get; }
    }

    /// <summary>
    /// DataModel 存取工具的基底介面，儲存用來設定資料源的 DataManagerActivator 以及建立資料管理器的 DataManagerBuilder
    /// <para>可以在需要時才建立並活化資料管理器</para>
    /// </summary>
    public interface IModelHelper<DataModel> : IModelHelper where DataModel : class
    {
        /// <summary>
        /// 預設的主要資料管理器
        /// </summary>
        /// <returns>活性化後的資料管理器，如果活性化失敗，回傳 null</returns>
        IDataManager<DataModel>? DefaultDataManager { get; set; }
    }
}

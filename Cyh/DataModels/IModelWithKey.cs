namespace Cyh.DataModels
{
    /// <summary>
    /// 含有鍵值的資料模型
    /// </summary>
    public interface IModelWithKey
    {
        /// <summary>
        /// 取得鍵值
        /// </summary>
        /// <returns>該模型的鍵值</returns>
        public object GetKey();
    }
}

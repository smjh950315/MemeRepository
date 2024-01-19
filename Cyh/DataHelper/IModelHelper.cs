namespace Cyh.DataHelper
{
    /// <summary>
    /// 處理Model相關的介面 
    /// </summary>
    /// <typeparam name="TModel">要處理的Model</typeparam>
    public interface IModelHelper<TModel> where TModel : class
    {
        /// <summary>
        /// 取得Model屬性名稱的清單
        /// </summary>
        /// <returns>Model屬性名稱的清單</returns>
        List<string> GetMemberList();

        /// <summary>
        /// 將模型 src 內名稱是 name 的成員值設定到 dst
        /// </summary>
        /// <param name="_src">輸入的 Model</param>
        /// <param name="_name">目標成員名稱</param>
        /// <param name="_dst">輸出值</param>
        /// <returns>是否成功取得</returns>
        bool TryGetValue(out object? _dst, TModel? _src, string _name);

        /// <summary>
        /// 將 src 值設定到到模型 dst 內名稱是 name 的成員
        /// </summary>
        /// <param name="_val">輸入的值</param>
        /// <param name="_name">目標成員名稱</param>
        /// <param name="_dst">要設定值的 Model</param>
        /// <returns>是否成功設定</returns>
        bool TrySetValue(ref TModel? _dst, string _name, object? _val);
    }
}

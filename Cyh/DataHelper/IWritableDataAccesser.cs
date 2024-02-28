using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料寫入器
    /// </summary>
    public interface IWritableDataAccesser
    {
        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        bool IsAccessable { get; }

        /// <summary>
        /// 存取的資料型別
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// 空的結果
        /// </summary>
        IDataTransResult EmptyResult { get; }

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInput">要寫入資料</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行成功與否，如不選擇立即執行，則回傳 TRUE 表示已加入貯列</returns>
        bool TryAddOrUpdateSingle(object? dataInput, IDataTransResult? dataTransResult, bool execNow);

        /// <summary>
        /// 嘗試移除資料
        /// </summary>
        /// <param name="expression">移除用的條件式</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行成功與否，如不選擇立即執行，則回傳 TRUE 表示已加入貯列</returns>
        bool TryRemoveSingle(object? dataRemove, IDataTransResult? dataTransResult, bool execNow);

        /// <summary>
        /// 處理例外
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception? exception);
    }

    /// <summary>
    /// 資料寫入器
    /// </summary>
    /// <typeparam name="T">要寫入的資料型別</typeparam>
    public interface IWritableDataAccesser<T> : IWritableDataAccesser
    {
        /// <summary>
        /// 嘗試寫入或更新資料
        /// <para>如不選擇立即執行，將輸入資料加入貯列</para>
        /// <para>如選擇立即執行，將當前與更早之前加入但是還未儲存的資料進行儲存</para>
        /// </summary>
        /// <param name="data">要寫入的資料</param>
        /// <param name="dataTransResult">資料交易明細</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        bool TryAddOrUpdate(T? data, IDataTransResult? dataTransResult, bool execNow);

        /// <summary>
        /// 嘗試寫入或更新資料
        /// <para>如不選擇立即執行，將輸入資料加入貯列</para>
        /// <para>如選擇立即執行，將當前與更早之前加入但是還未儲存的資料進行儲存</para>
        /// </summary>
        /// <param name="dataInput">要寫入的資料清單</param>
        /// <param name="dataTransResult">資料交易明細</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        bool TryAddOrUpdate(IEnumerable<T> dataInput, IDataTransResult? dataTransResult, bool execNow);

        /// <summary>
        /// 嘗試移除資料
        /// </summary>
        /// <param name="data">要移除的資料</param>
        /// <param name="dataTransResult">資料交易明細</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        bool TryRemove(T? data, IDataTransResult? dataTransResult, bool execNow);

        /// <summary>
        /// 嘗試移除資料
        /// </summary>
        /// <param name="expression">判斷式</param>
        /// <param name="dataTransResult">資料交易明細</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        bool TryRemove(Expression<Func<T, bool>> expression, IDataTransResult? dataTransResult, bool execNow);

        /// <summary>
        /// 嘗試更新資料
        /// </summary>
        /// <param name="dataTransResult">資料交易明細</param>
        /// <returns>是否成功</returns>
        bool ApplyChanges(IDataTransResult? dataTransResult);
    }
}

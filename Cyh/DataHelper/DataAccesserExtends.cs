using Cyh.DataModels;
using System.Collections;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    public static class DataAccesserExtends
    {
        /// <summary>
        /// 可以存取到的資料數量
        /// </summary>
        /// <param name="filter">篩選條件</param>
        /// <typeparam name="T">資料類型</typeparam>
        /// <returns>資料數量</returns>
        public static int Count<T>(this IReadOnlyDataAccesser<T> readOnlyDataAccesser, Expression<Func<T, bool>>? filter = null) {
            return CheckNullAccesserHelper.Count(readOnlyDataAccesser, filter);
        }

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInputs">要寫入資料集合</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult? TryAddOrUpdate(this IWritableDataAccesser writable, IEnumerable? dataInputs, IDataTransResult? dataTransResult, bool execNow) {
            if (writable == null)
                return null;
            if (dataInputs == null)
                return writable.EmptyResult;
            IDataTransResult result = writable.EmptyResult;

            foreach (object? item in dataInputs) {
                result.TotalTransCount++;
                if (writable.TryAddOrUpdateSingle(item, dataTransResult, execNow))
                    result.SucceedTransCount++;
            }
            return result;
        }

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <typeparam name="T">要寫入的資料類型</typeparam>
        /// <param name="dataInputs">要寫入單個資料</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static bool TryAddOrUpdateSingleT<T>(this IWritableDataAccesser<T> writable, object? dataInputs, IDataTransResult? dataTransResult, bool execNow) {
            if (writable == null || dataInputs == null)
                return false;
            try {
                T temp = (T)dataInputs;

                if (temp == null)
                    return false;
                return writable.TryAddOrUpdate(temp, dataTransResult, execNow);
            } catch (Exception? ex) {
                writable.HandleException(ex);
                return !execNow;
            }
        }

        /// <summary>
        /// 嘗試移除資料
        /// </summary>
        /// <typeparam name="T">要移除的資料類型</typeparam>
        /// <param name="dataInputs">要移除單個資料</param>
        /// <param name="dataTransResult">執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行結果</returns>
        public static bool TryRemoveSingleT<T>(this IWritableDataAccesser<T> writable, object? dataInputs, IDataTransResult? dataTransResult, bool execNow) {
            if (writable == null || dataInputs == null)
                return false;
            try {
                T temp = (T)dataInputs;

                if (temp == null)
                    return false;
                return writable.TryRemove(temp, dataTransResult, execNow);
            } catch (Exception? ex) {
                writable.HandleException(ex);
                return !execNow;
            }
        }
    }
}

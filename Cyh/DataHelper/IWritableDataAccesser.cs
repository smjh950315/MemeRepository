using Cyh.DataModels;
using System.Collections;
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
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>執行成功與否，如不選擇立即執行，則回傳 TRUE 表示已加入貯列</returns>
        bool TryAddOrUpdateSingle(object? dataInput, IDataTransResult? prevResult, bool execNow);

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
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        bool TryAddOrUpdate(T? data, IDataTransResult? prevResult, bool execNow);

        /// <summary>
        /// 嘗試寫入或更新資料
        /// <para>如不選擇立即執行，將輸入資料加入貯列</para>
        /// <para>如選擇立即執行，將當前與更早之前加入但是還未儲存的資料進行儲存</para>
        /// </summary>
        /// <param name="dataInput">要寫入的資料清單</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        bool TryAddOrUpdate(IEnumerable<T> dataInput, IDataTransResult? prevResult, bool execNow);

        bool ApplyChanges(IDataTransResult? prevResult);
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInputs">要寫入資料集合</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult? TryAddOrUpdate(this IWritableDataAccesser writable, IEnumerable? dataInputs, IDataTransResult? prevResult, bool execNow) {
            if (writable == null)
                return null;
            if (dataInputs == null)
                return writable.EmptyResult;
            IDataTransResult result = writable.EmptyResult;

            foreach (object? item in dataInputs) {
                result.TotalTransCount++;
                if (writable.TryAddOrUpdateSingle(item, prevResult, execNow))
                    result.SucceedTransCount++;
            }
            return result;
        }

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInputs">要寫入單個資料</param>
        /// <typeparam name="T">要寫入的資料類型</typeparam>
        /// <returns>執行結果</returns>
        public static bool TryAddOrUpdateSingleT<T>(this IWritableDataAccesser<T> writable, object? dataInputs, IDataTransResult? prevResult, bool execNow) {
            if (writable == null || dataInputs == null)
                return false;
            try {
                T temp = (T)dataInputs;

                if (temp == null)
                    return false;
                return writable.TryAddOrUpdate(temp, prevResult, execNow);
            } catch (Exception? ex) {
                writable.HandleException(ex);
                return false;
            }
        }
    }

    public static partial class MyDataHelperExtends
    {
        public static bool __CheckNullAnd_SaveDataToAccesser<T>(IWritableDataAccesser<T>? writableDataAccesser, T? data, IDataTransResult? prevResult, bool execNow) {

            if (writableDataAccesser == null) {
                // 無效的資料源頭，交易鎖定
                prevResult.TryAppendError_DataAccesserNotInit();
                prevResult.BatchOnFinish(false);
                return false;
            }

            return writableDataAccesser.TryAddOrUpdate(data, prevResult, execNow);
        }

        public static bool __CheckNullAnd_SaveDatasToAccesser<T>(IWritableDataAccesser<T>? writableDataAccesser, IEnumerable<T> datas, IDataTransResult? prevResult, bool execNow) {

            if (writableDataAccesser == null) {
                // 無效的資料源頭，交易鎖定
                prevResult.TryAppendError_DataAccesserNotInit();
                return false;
            }

            return writableDataAccesser.TryAddOrUpdate(datas, prevResult, execNow);
        }

        public static bool __CheckNullAnd_SaveDataToAccesserFrom<T, U>(IWritableDataAccesser<T>? writableDataAccesser, Expression<Func<U, T>>? selector, U? data, IDataTransResult? prevResult, bool execNow) {

            if (writableDataAccesser == null) {
                // 無效的資料源頭，交易鎖定
                prevResult.TryAppendError_DataAccesserNotInit();
                prevResult.BatchOnFinish(false);
                return false;
            }

            if (selector == null) {
                // 資料轉換器失效，後續會受到影響，交易鎖定
                prevResult.TryAppendError_CannotConvertInput();
                prevResult.BatchOnFinish(false);
                return false;
            }

            Func<U, T>? converter;
            try {
                converter = selector.Compile();
            } catch {
                // 資料轉換器無法建立，後續會受到影響，交易鎖定
                prevResult.TryAppendError_CannotConvertInput();
                prevResult.BatchOnFinish(false);
                return false;
            }

            T? temp;
            try {
#pragma warning disable CS8604
                temp = converter(data);
#pragma warning restore CS8604
            } catch {
                temp = default;
            }

            return writableDataAccesser.TryAddOrUpdate(temp, prevResult, execNow);
        }

        public static bool __CheckNullAnd_SaveDatasToAccesserFrom<T, U>(IWritableDataAccesser<T>? writableDataAccesser, Expression<Func<U, T>>? selector, IEnumerable<U> datas, IDataTransResult? prevResult, bool execNow) {

            if (writableDataAccesser == null) {
                // 無效的資料源頭，交易鎖定
                prevResult.TryAppendError_DataAccesserNotInit();
                prevResult.BatchOnFinish(false);
                return false;
            }
            if (selector == null) {
                // 資料轉換器失效，後續會受到影響，交易鎖定
                prevResult.TryAppendError_CannotConvertInput();
                prevResult.BatchOnFinish(false);
                return false;
            }

            Func<U, T>? converter;
            try {
                converter = selector.Compile();
            } catch {
                // 資料轉換器無法建立，後續會受到影響，交易鎖定
                prevResult.TryAppendError_CannotConvertInput();
                prevResult.BatchOnFinish(false);
                return false;
            }

            foreach (U? data in datas) {
                writableDataAccesser.TryAddOrUpdate(converter(data), prevResult, false);
            }

            return writableDataAccesser.ApplyChanges(prevResult);
        }
    }
}

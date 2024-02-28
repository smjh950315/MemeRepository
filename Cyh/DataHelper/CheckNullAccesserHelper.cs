using Cyh.DataHelper;
using Cyh.DataModels;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    internal static class CheckNullAccesserHelper
    {
        private static bool TerminateByInvalidSource([NotNullWhen(false)] IWritableDataAccesser? writableDataAccesser, IDataTransResult? dataTransResult) {
            if (writableDataAccesser == null) {
                // 無效的資料源頭，交易鎖定
                dataTransResult.TryAppendError_DataAccesserNotInit();
                dataTransResult.BatchOnFinish(false);
                return true;
            } else {
                return false;
            }
        }
        private static bool TerminateByInvalidConverter<T, U>([NotNullWhen(false)] Expression<Func<U, T>>? selector, IDataTransResult? dataTransResult, [NotNullWhen(false)] out Func<U, T>? converter) {
            if (selector == null) {
                // 資料轉換器失效，後續會受到影響，交易鎖定
                dataTransResult.TryAppendError_CannotConvertInput();
                dataTransResult.BatchOnFinish(false);
                converter = null;
                return true;
            }

            try {
                converter = selector.Compile();
            } catch {
                // 資料轉換器無法建立，後續會受到影響，交易鎖定
                dataTransResult.TryAppendError_CannotConvertInput();
                dataTransResult.BatchOnFinish(false);
                converter = null;
                return true;
            }
            return false;
        }

        #region READONLY
        internal static int Count<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, bool>>? filter = null) {
            if (readOnlyDataAccesser == null)
                return 0;
            if (!readOnlyDataAccesser.IsAccessable || readOnlyDataAccesser.Queryable == null)
                return 0;
            if (filter == null)
                return readOnlyDataAccesser.Queryable.Count();
            else
                return readOnlyDataAccesser.Queryable.Count(filter);
        }

        internal static bool Exist<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, bool>>? filter = null) {
            if (readOnlyDataAccesser == null) { return false; }
            if (readOnlyDataAccesser.Queryable == null) { return false; }
            if (filter == null) {
                return readOnlyDataAccesser.Queryable.Any();
            } else {
                return readOnlyDataAccesser.Queryable.Any(filter);
            }
        }

        internal static T? GetData<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetData(expression, dataTransResult) : default;
        }

        internal static IEnumerable<T> GetDatas<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatas(expression, dataTransResult) : Enumerable.Empty<T>();
        }

        internal static IEnumerable<T> GetDatas<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatas(begin, count, expression, dataTransResult) : Enumerable.Empty<T>();
        }

        internal static U? GetDataAs<T, U>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, U>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDataAs(selector, expression, dataTransResult) : default;
        }

        internal static IEnumerable<U> GetDatasAs<T, U>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, U>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatasAs(selector, expression, dataTransResult) : Enumerable.Empty<U>();
        }

        internal static IEnumerable<U> GetDatasAs<T, U>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, U>>? selector, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatasAs(selector, begin, count, expression, dataTransResult) : Enumerable.Empty<U>();
        }
        #endregion

        #region WRITE
        internal static bool SaveData<T>(IWritableDataAccesser<T>? writableDataAccesser, T? data, IDataTransResult? dataTransResult, bool execNow) {
            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }
            return writableDataAccesser.TryAddOrUpdate(data, dataTransResult, execNow);
        }

        internal static bool SaveDatas<T>(IWritableDataAccesser<T>? writableDataAccesser, IEnumerable<T>? datas, IDataTransResult? dataTransResult, bool execNow) {
            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }
            return writableDataAccesser.TryAddOrUpdate(datas ?? Enumerable.Empty<T>(), dataTransResult, execNow);
        }

        internal static bool SaveDataFrom<T, U>(IWritableDataAccesser<T>? writableDataAccesser, Expression<Func<U, T>>? selector, U? data, IDataTransResult? dataTransResult, bool execNow) {

            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }

            Func<U, T>? converter;
            if (TerminateByInvalidConverter(selector, dataTransResult, out converter)) { return false; }

            T temp;
            try {
#pragma warning disable CS8604
                temp = converter(data);
#pragma warning restore CS8604
            } catch {
                return !execNow;
            }

            return writableDataAccesser.TryAddOrUpdate(temp, dataTransResult, execNow);
        }

        internal static bool SaveDatasFrom<T, U>(IWritableDataAccesser<T>? writableDataAccesser, Expression<Func<U, T>>? selector, IEnumerable<U> datas, IDataTransResult? dataTransResult, bool execNow) {

            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }

            Func<U, T>? converter;
            if (TerminateByInvalidConverter(selector, dataTransResult, out converter)) { return false; }

            foreach (U data in datas) {
                writableDataAccesser.TryAddOrUpdate(converter(data), dataTransResult, false);
            }

            return !execNow || writableDataAccesser.ApplyChanges(dataTransResult);
        }

        internal static bool RemoveData<T>(IWritableDataAccesser<T>? writableDataAccesser, T? dataModel, IDataTransResult? dataTransResult, bool execNow) {
            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }

            return writableDataAccesser.TryRemove(dataModel, dataTransResult, execNow);
        }

        internal static bool RemoveDatasBy<T>(IWritableDataAccesser<T>? writableDataAccesser, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult, bool execNow) {
            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }

            // 沒有指定條件，很危險，拒絕!
            if (expression == null) { return false; }

            return writableDataAccesser.TryRemove(expression, dataTransResult, execNow);
        }

        internal static bool RemoveDataFrom<T, U>(IWritableDataAccesser<T>? writableDataAccesser, Expression<Func<U, T>>? selector, U? data, IDataTransResult? dataTransResult, bool execNow) {

            if (TerminateByInvalidSource(writableDataAccesser, dataTransResult)) { return false; }

            Func<U, T>? converter;
            if (TerminateByInvalidConverter(selector, dataTransResult, out converter)) { return false; }

            T temp;
            try {
#pragma warning disable CS8604
                temp = converter(data);
#pragma warning restore CS8604
            } catch {
                return !execNow;
            }

            return writableDataAccesser.TryRemove(temp, dataTransResult, execNow);
        }
        #endregion
    }
}

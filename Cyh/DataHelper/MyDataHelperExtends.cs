using System.Collections;
using System.Linq.Expressions;
using Cyh.DataHelper;
using Cyh.MyException;

namespace Cyh.DataHelper
{
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 資料存取器驗證
        /// </summary>
        /// <returns>資料存取器的資料來源是否有效</returns>
        public static bool DataSourceIsValid<T>(this IMyDataAccesser? myDataAccesser) {
            return myDataAccesser?.IsAccessable ?? false;
        }

        /// <summary>
        /// 嘗試寫入或更新資料
        /// </summary>
        /// <param name="dataInputs">要寫入資料集合</param>
        /// <returns>執行結果</returns>
        public static IDataTransResult? TryAddOrUpdate(this IWritableDataAccesser writable,
            IEnumerable? dataInputs) {
            if (writable == null)
                return null;
            if (dataInputs == null)
                return writable.EmptyResult;
            IDataTransResult result = writable.EmptyResult;

            foreach (object? item in dataInputs) {
                result.TotalTransCount++;
                if (writable.TryAddOrUpdateSingle(item))
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
        public static bool TryAddOrUpdateSingleT<T>(this IWritableDataAccesser<T> writable,
            object? dataInputs) {
            if (writable == null || dataInputs == null)
                return false;
            try {
                T temp = (T)dataInputs;

                if (temp == null)
                    return false;
                return writable.TryAddOrUpdate(temp);
            } catch (Exception? ex) {
                writable.HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// 此資料元涵蓋的範圍數量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>可查詢數</returns>
        public static int Count<T>(this IReadOnlyDataAccesser<T> src) {
            if (src == null)
                return 0;
            if (src.Queryable == null)
                return 0;
            try {
                return src.Queryable.Count();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return 0;
            }
        }

        /// <summary>
        /// 用條件取得資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static IEnumerable<T> TryGetDatas<T>(this IReadOnlyDataAccesser<T> src,
            Expression<Func<T, bool>>? filter = null) {
            if (src?.Queryable == null)
                return Enumerable.Empty<T>();
            try {
                if (filter == null)
                    return src.Queryable.ToList();
                return src.Queryable.Where(filter).ToList();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 用條件取得資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static IEnumerable<T> TryGetDatas<T>(this IReadOnlyDataAccesser<T> src,
            int indexBegin, int count, Expression<Func<T, bool>>? filter = null) {
            if (src?.Queryable == null)
                return Enumerable.Empty<T>();
            try {
                if (filter == null)
                    return src.Queryable?.Skip(indexBegin).Take(count).ToList() ?? Enumerable.Empty<T>();
                return src.Queryable.Where(filter).Skip(indexBegin).Take(count).ToList();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 用條件嘗試取得單筆資料
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static T? TryGetData<T>(this IReadOnlyDataAccesser<T> src,
            Expression<Func<T, bool>>? filter = null) {
            if (src?.Queryable == null)
                return default;
            try {
                if (filter == null)
                    return src.Queryable.FirstOrDefault() ?? default;
                return src.Queryable.Where(filter).FirstOrDefault() ?? default;
            } catch (Exception? ex) {
                src.HandleException(ex);
                return default;
            }
        }

        /// <summary>
        /// 用條件取得資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static IEnumerable<T> TryGetObjDatas<T>(this IReadOnlyDataAccesser<T> src,
            int indexBegin, int count, Expression? filter = null) {
            if (src == null)
                return Enumerable.Empty<T>();
            try {
                if (filter == null)
                    return src.Queryable?.Skip(indexBegin).Take(count).ToList() ?? Enumerable.Empty<T>();
                var filterT = filter as Expression<Func<T, bool>>;
                if (filterT == null)
                    return Enumerable.Empty<T>();
                return src?.Queryable?.Where(filterT).Skip(indexBegin).Take(count) ?? Enumerable.Empty<T>();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 用條件嘗試取得單筆資料
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static T? TryGetObjData<T>(this IReadOnlyDataAccesser<T> src,
            Expression? filter = null) {
            if (src == null)
                return default;
            if (src.Queryable == null)
                return default;
            try {
                if (filter == null)
                    return src.Queryable.FirstOrDefault() ?? default;                
                var filterT = filter as Expression<Func<T, bool>>;
                if (filterT == null)
                    return default;
                return src.Queryable.Where(filterT).FirstOrDefault();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return default;
            }
        }

        /// <summary>
        /// 用條件取得資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static IEnumerable<TOut> TryGetDatasAs<T, TOut>(this IReadOnlyDataAccesser<T> src,
            Expression<Func<T, TOut>>? selctor, Expression<Func<T, bool>>? filter = null) {
            if (src?.Queryable == null || selctor == null)
                return Enumerable.Empty<TOut>();
            try {
                if (filter == null)
                    return src.Queryable.Select(selctor).ToList();
                return src.Queryable.Where(filter).Select(selctor).ToList();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return Enumerable.Empty<TOut>();
            }
        }

        /// <summary>
        /// 用條件取得資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static IEnumerable<TOut> TryGetDatasAs<T, TOut>(this IReadOnlyDataAccesser<T> src,
            Expression<Func<T, TOut>>? selctor, int indexBegin, int count, Expression<Func<T, bool>>? filter = null) {
            if (src?.Queryable == null || selctor == null)
                return Enumerable.Empty<TOut>();
            try {
                if (filter == null)
                    return src.Queryable?.Skip(indexBegin).Take(count).Select(selctor).ToList() ?? Enumerable.Empty<TOut>();
                return src.Queryable.Where(filter).Skip(indexBegin).Take(count).Select(selctor).ToList();
            } catch (Exception? ex) {
                src.HandleException(ex);
                return Enumerable.Empty<TOut>();
            }
        }

        /// <summary>
        /// 用條件嘗試取得單筆資料
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>取得的資料</returns>
        public static TOut? TryGetDataAs<T, TOut>(this IReadOnlyDataAccesser<T> src,
            Expression<Func<T, TOut>>? selctor, Expression<Func<T, bool>>? filter = null) {
            if (src?.Queryable == null || selctor == null)
                return default;
            try {
                if (filter == null)
                    return src.Queryable.Select(selctor).FirstOrDefault() ?? default;
                return src.Queryable.Where(filter).Select(selctor).FirstOrDefault() ?? default;
            } catch (Exception? ex) {
                src.HandleException(ex);
                return default;
            }
        }
    }
}

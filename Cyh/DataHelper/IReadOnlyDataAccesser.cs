using System.Linq.Expressions;
using Cyh.DataModels;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料讀取器
    /// </summary>
    public interface IReadOnlyDataAccesser
    {
        /// <summary>
        /// 存取的資料型別
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// 當前的資料來源是否可以存取
        /// </summary>
        bool IsAccessable { get; }

        /// <summary>
        /// 處理例外
        /// </summary>
        /// <param name="exception"></param>
        void HandleException(Exception? exception);
    }

    /// <summary>
    /// 資料讀取器
    /// </summary>
    /// <typeparam name="T">要讀取的資料型別</typeparam>
    public interface IReadOnlyDataAccesser<T> : IReadOnlyDataAccesser
    {
        /// <summary>
        /// 資料源查詢的介面
        /// </summary>
        IQueryable<T>? Queryable { get; }

        /// <summary>
        /// 用條件嘗試取得單筆資料
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>取得的資料</returns>
        T? TryGetData(Expression<Func<T, bool>>? filter, IDataTransResult? dataTransResult);

        /// <summary>
        /// 用條件取得資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>取得的資料</returns>
        IEnumerable<T> TryGetDatas(Expression<Func<T, bool>>? filter, IDataTransResult? dataTransResult);

        /// <summary>
        /// 用條件以取得特定範圍筆數的資料集合
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>取得的資料</returns>
        IEnumerable<T> TryGetDatas(int begin, int count, Expression<Func<T, bool>>? filter, IDataTransResult? dataTransResult);

        /// <summary>
        /// 用條件嘗試取得單筆資料
        /// </summary>
        /// <param name="filter">驗證用的敘述式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>取得的資料</returns>
        TOut? TryGetDataAs<TOut>(Expression<Func<T, TOut>>? selctor, Expression<Func<T, bool>>? filter, IDataTransResult? dataTransResult);

        /// <summary>
        /// 用條件取得並轉換成另一類型的資料集合
        /// </summary>
        /// <param name="selctor">轉換到目標類型的轉換LINQ語法</param>
        /// <param name="filter">驗證用的敘述式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <typeparam name="TOut">目標資料類型</typeparam>
        /// <returns>目標資料類型集合</returns>
        IEnumerable<TOut> TryGetDatasAs<TOut>(Expression<Func<T, TOut>>? selctor, Expression<Func<T, bool>>? filter, IDataTransResult? dataTransResult);

        /// <summary>
        /// 用條件以取得特定範圍筆數並轉換成另一類型的資料集合
        /// </summary>
        /// <param name="selctor">轉換到目標Model的轉換LINQ語法</param>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="filter">驗證用的敘述式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <typeparam name="TOut">目標資料類型</typeparam>
        /// <returns>目標資料類型集合</returns>
        IEnumerable<TOut> TryGetDatasAs<TOut>(Expression<Func<T, TOut>>? selctor, int begin, int count, Expression<Func<T, bool>>? filter, IDataTransResult? dataTransResult);
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 可以存取到的資料數量
        /// </summary>
        /// <typeparam name="T">資料類型</typeparam>
        /// <returns>資料數量</returns>
        public static int Count<T>(this IReadOnlyDataAccesser<T>? readOnlyDataAccesser) {
            if (readOnlyDataAccesser == null)
                return 0;
            if (!readOnlyDataAccesser.IsAccessable || readOnlyDataAccesser.Queryable == null)
                return 0;
            return readOnlyDataAccesser.Queryable.Count();
        }
    }

    public static partial class MyDataHelperExtends
    {
        private static T? __CheckNullAnd_GetDataFromAccesser<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetData(expression, dataTransResult) : default;
        }

        private static IEnumerable<T> __CheckNullAnd_GetDatasFromAccesser<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatas(expression, dataTransResult) : Enumerable.Empty<T>();
        }

        private static IEnumerable<T> __CheckNullAnd_GetDatasFromAccesser<T>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatas(begin, count, expression, dataTransResult) : Enumerable.Empty<T>();
        }

        private static U? __CheckNullAnd_GetDataFromAccesserAs<T, U>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, U>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDataAs(selector, expression, dataTransResult) : default;
        }

        private static IEnumerable<U> __CheckNullAnd_GetDatasFromAccesserAs<T, U>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, U>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatasAs(selector, expression, dataTransResult) : Enumerable.Empty<U>();
        }

        private static IEnumerable<U> __CheckNullAnd_GetDatasFromAccesserAs<T, U>(IReadOnlyDataAccesser<T>? readOnlyDataAccesser, Expression<Func<T, U>>? selector, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return readOnlyDataAccesser != null ? readOnlyDataAccesser.TryGetDatasAs(selector, begin, count, expression, dataTransResult) : Enumerable.Empty<U>();
        }
    }
}

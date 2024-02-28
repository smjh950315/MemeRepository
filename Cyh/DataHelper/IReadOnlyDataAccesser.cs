using Cyh.DataModels;
using System.Linq.Expressions;
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

        bool Any(Expression<Func<T, bool>>? predicate);

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
}

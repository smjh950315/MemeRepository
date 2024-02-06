using Cyh.DataModels;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    public interface IDataManager
    {
    }

    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    /// <typeparam name="T">資料的模型</typeparam>
    public interface IDataManager<T> : IDataManager
    {
        /// <summary>
        /// 主要資料來源
        /// </summary>
        IMyDataAccesser<T>? MainDataSource { get; set; }
    }

    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    /// <typeparam name="T">主要資料的模型</typeparam>
    /// <typeparam name="U">次要資料的模型</typeparam>
    public interface IDataManager<T, U> : IDataManager<T>
    {
        /// <summary>
        /// 次要資料的來源
        /// </summary>
        IMyDataAccesser<U>? SubDataSource { get; set; }
    }

    /// <summary>
    /// 資料管理器的介面
    /// </summary>
    /// <typeparam name="T">主要資料的模型</typeparam>
    /// <typeparam name="U">次要資料的模型1</typeparam>
    /// <typeparam name="V">次要資料的模型2</typeparam>
    public interface IDataManager<T, U, V> : IDataManager<T, U>
    {
        /// <summary>
        /// 次要資料2的來源
        /// </summary>
        IMyDataAccesser<V>? SubDataSource2 { get; set; }
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="T">資料的模型</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<T>([NotNullWhen(true)] this IDataManager<T>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainDataSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<T, U>([NotNullWhen(true)] this IDataManager<T, U>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainDataSource != null && formMgr.SubDataSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<T, U, V>([NotNullWhen(true)] this IDataManager<T, U, V>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainDataSource != null && formMgr.SubDataSource != null && formMgr.SubDataSource2 != null;
        }
    }

    // T,U,V get single data
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 取得主要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="expression">主要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料</returns>
        public static T? GetData<T>(this IDataManager<T>? dataManager, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDataFromAccesser(dataManager?.MainDataSource, expression, dataTransResult);
        }

        /// <summary>
        /// 取得次要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料</returns>
        public static U? GetData<T, U>(this IDataManager<T, U>? dataManager, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDataFromAccesser(dataManager?.SubDataSource, expression, dataTransResult);
        }

        /// <summary>
        /// 取得次要資料2
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料</returns>
        public static V? GetData<T, U, V>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDataFromAccesser(dataManager?.SubDataSource2, expression, dataTransResult);
        }
    }
    // T,U,V get single diff data
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 以另一種資料模型取得主要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="expression">主要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料轉換而成的目的資料模型</returns>
        public static P? GetDataAs<T, P>(this IDataManager<T>? dataManager, Expression<Func<T, P>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDataFromAccesserAs(dataManager?.MainDataSource, selector, expression, dataTransResult);
        }
        /// <summary>
        /// 以另一種資料模型取得次要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="expression">次要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型</returns>
        public static P? GetDataAs<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<U, P>>? selector, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDataFromAccesserAs(dataManager?.SubDataSource, selector, expression, dataTransResult);
        }
        /// <summary>
        /// 以另一種資料模型取得次要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="expression">次要資料2的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料2轉換而成的目的資料模型</returns>
        public static P? GetDataAs<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, P>>? selector, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDataFromAccesserAs(dataManager?.SubDataSource2, selector, expression, dataTransResult);
        }
    }

    // T,U,V get multiple data with range
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 取得主要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">主要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料之集合</returns>
        public static IEnumerable<T> GetDatas<T>(this IDataManager<T>? dataManager, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesser(dataManager?.MainDataSource, begin, count, expression, dataTransResult);
        }

        /// <summary>
        /// 取得次要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<U> GetDatas<T, U>(this IDataManager<T, U>? dataManager, int begin, int count, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesser(dataManager?.SubDataSource, begin, count, expression, dataTransResult);
        }

        /// <summary>
        /// 取得次要資料2之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<V> GetDatas<T, U, V>(this IDataManager<T, U, V>? dataManager, int begin, int count, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesser(dataManager?.SubDataSource2, begin, count, expression, dataTransResult);
        }
    }
    // T,U,V get multiple diff data with range
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 以另一種資料模型取得主要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">主要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, P>(this IDataManager<T>? dataManager, Expression<Func<T, P>>? selector, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesserAs(dataManager?.MainDataSource, selector, begin, count, expression, dataTransResult);
        }

        /// <summary>
        /// 以另一種資料模型取得次要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<U, P>>? selector, int begin, int count, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesserAs(dataManager?.SubDataSource, selector, begin, count, expression, dataTransResult);
        }

        /// <summary>
        /// 以另一種資料模型取得次要資料2之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, P>>? selector, int begin, int count, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesserAs(dataManager?.SubDataSource2, selector, begin, count, expression, dataTransResult);
        }
    }

    // T,U,V get multiple data without range 
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 取得主要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="expression">主要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料之集合</returns>
        public static IEnumerable<T> GetDatas<T>(this IDataManager<T>? dataManager, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesser(dataManager?.MainDataSource, expression, dataTransResult);
        }

        /// <summary>
        /// 取得次要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<U> GetDatas<T, U>(this IDataManager<T, U>? dataManager, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesser(dataManager?.SubDataSource, expression, dataTransResult);
        }

        /// <summary>
        /// 取得次要資料2之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<V> GetDatas<T, U, V>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesser(dataManager?.SubDataSource2, expression, dataTransResult);
        }
    }
    // T,U,V get multiple diff data without range
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 以另一種資料模型取得主要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="expression">主要資料的篩選條件</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, P>(this IDataManager<T>? dataManager, Expression<Func<T, P>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesserAs(dataManager?.MainDataSource, selector, expression, dataTransResult);
        }

        /// <summary>
        /// 以另一種資料模型取得次要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<U, P>>? selector, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesserAs(dataManager?.SubDataSource, selector, expression, dataTransResult);
        }

        /// <summary>
        /// 以另一種資料模型取得次要資料2之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="expression">條件表達式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, P>>? selector, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult) {
            return __CheckNullAnd_GetDatasFromAccesserAs(dataManager?.SubDataSource2, selector, expression, dataTransResult);
        }
    }


    // T,U,V save single data
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 儲存主要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveData<T>(this IDataManager<T>? dataManager, T? data, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDataToAccesser(dataManager?.MainDataSource, data, dataTransResult, execNow);
        }

        /// <summary>
        /// 儲存次要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveData<T, U>(this IDataManager<T, U>? dataManager, U? data, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDataToAccesser(dataManager?.SubDataSource, data, dataTransResult, execNow);
        }

        /// <summary>
        /// 儲存次要資料2
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveData<T, U, V>(this IDataManager<T, U, V>? dataManager, V? data, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDataToAccesser(dataManager?.SubDataSource2, data, dataTransResult, execNow);
        }
    }
    // T,U,V save single data from diff
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 以另一種資料模型儲存主要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">來源資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDataFrom<T, P>(this IDataManager<T>? dataManager, Expression<Func<P, T>>? selector, P? data, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDataToAccesserFrom(dataManager?.MainDataSource, selector, data, dataTransResult, execNow);
        }

        /// <summary>
        /// 以另一種資料模型儲存次要資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">來源資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDataFrom<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<P, U>>? selector, P? data, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDataToAccesserFrom(dataManager?.SubDataSource, selector, data, dataTransResult, execNow);
        }

        /// <summary>
        /// 以另一種資料模型儲存次要資料2
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">來源資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDataFrom<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<P, V>>? selector, P? data, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDataToAccesserFrom(dataManager?.SubDataSource2, selector, data, dataTransResult, execNow);
        }
    }
    // T,U,V save multiple data
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 儲存主要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatas<T>(this IDataManager<T>? dataManager, IEnumerable<T> datas, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDatasToAccesser(dataManager?.MainDataSource, datas, dataTransResult, execNow);
        }

        /// <summary>
        /// 儲存次要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatas<T, U>(this IDataManager<T, U>? dataManager, IEnumerable<U> datas, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDatasToAccesser(dataManager?.SubDataSource, datas, dataTransResult, execNow);
        }

        /// <summary>
        /// 儲存次要資料2之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatas<T, U, V>(this IDataManager<T, U, V>? dataManager, IEnumerable<V> datas, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDatasToAccesser(dataManager?.SubDataSource2, datas, dataTransResult, execNow);
        }
    }
    // T,U,V save multiple data from diff
    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 以另一種資料模型儲存主要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">次要資料的模型</typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatasFrom<T, P>(this IDataManager<T>? dataManager, Expression<Func<P, T>>? selector, IEnumerable<P> datas, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDatasToAccesserFrom(dataManager?.MainDataSource, selector, datas, dataTransResult, execNow);
        }

        /// <summary>
        /// 以另一種資料模型儲存次要資料之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatasFrom<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<P, U>>? selector, IEnumerable<P> datas, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDatasToAccesserFrom(dataManager?.SubDataSource, selector, datas, dataTransResult, execNow);
        }

        /// <summary>
        /// 以另一種資料模型儲存次要資料2之集合
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="selector">資料轉換器(LINQ)</param>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatasFrom<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<P, V>>? selector, IEnumerable<P> datas, IDataTransResult? dataTransResult, bool execNow) {
            return __CheckNullAnd_SaveDatasToAccesserFrom(dataManager?.SubDataSource2, selector, datas, dataTransResult, execNow);
        }
    }
}

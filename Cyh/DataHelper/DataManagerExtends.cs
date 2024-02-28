using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    public static class DataManagerExtends
    {
        /// <summary>
        /// 是否有任何符合查詢條件式的資料
        /// </summary>
        /// <typeparam name="T">要查詢的模型</typeparam>
        /// <param name="expression">查詢條件式</param>
        /// <returns>是否有任何相符的結果</returns>
        public static bool Any<T>(this IDataManager<T>? dataManager, Expression<Func<T, bool>>? expression)
            => CheckNullAccesserHelper.Exist(dataManager?.MainDataSource, expression);

        /// <summary>
        /// 是否有任何符合查詢條件式的資料
        /// </summary>
        /// <typeparam name="U">要查詢的模型</typeparam>
        /// <param name="expression">查詢條件式</param>
        /// <returns>是否有任何相符的結果</returns>
        public static bool Any<T, U>(this IDataManager<T, U>? dataManager, Expression<Func<U, bool>>? expression)
            => CheckNullAccesserHelper.Exist(dataManager?.SubDataSource, expression);

        /// <summary>
        /// 是否有任何符合查詢條件式的資料
        /// </summary>
        /// <typeparam name="U">要查詢的模型</typeparam>
        /// <param name="expression">查詢條件式</param>
        /// <returns>是否有任何相符的結果</returns>
        public static bool Any<T, U, V>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, bool>>? expression)
            => CheckNullAccesserHelper.Exist(dataManager?.SubDataSource2, expression);

        /// <summary>
        /// 取得符合條件的資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件資料</returns>
        public static T? GetData<T>(this IDataManager<T>? dataManager, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetData(dataManager?.MainDataSource, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件資料</returns>
        public static U? GetData<T, U>(this IDataManager<T, U>? dataManager, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetData(dataManager?.SubDataSource, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件資料</returns>
        public static V? GetData<T, U, V>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetData(dataManager?.SubDataSource2, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料之集合</returns>
        public static IEnumerable<T> GetDatas<T>(this IDataManager<T>? dataManager, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatas(dataManager?.MainDataSource, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<U> GetDatas<T, U>(this IDataManager<T, U>? dataManager, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatas(dataManager?.SubDataSource, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<V> GetDatas<T, U, V>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatas(dataManager?.SubDataSource2, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料(指定最大查詢數量)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料之集合</returns>
        public static IEnumerable<T> GetDatas<T>(this IDataManager<T>? dataManager, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatas(dataManager?.MainDataSource, begin, count, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料(指定最大查詢數量)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<U> GetDatas<T, U>(this IDataManager<T, U>? dataManager, int begin, int count, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatas(dataManager?.SubDataSource, begin, count, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料(指定最大查詢數量)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料之集合</returns>
        public static IEnumerable<V> GetDatas<T, U, V>(this IDataManager<T, U, V>? dataManager, int begin, int count, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatas(dataManager?.SubDataSource2, begin, count, expression, dataTransResult);

        /// <summary>
        /// 儲存資料
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
        public static bool SaveData<T>(this IDataManager<T>? dataManager, T? data, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveData(dataManager?.MainDataSource, data, dataTransResult, execNow);

        /// <summary>
        /// 儲存資料
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
        public static bool SaveData<T, U>(this IDataManager<T, U>? dataManager, U? data, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveData(dataManager?.SubDataSource, data, dataTransResult, execNow);

        /// <summary>
        /// 儲存資料
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
        public static bool SaveData<T, U, V>(this IDataManager<T, U, V>? dataManager, V? data, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveData(dataManager?.SubDataSource2, data, dataTransResult, execNow);

        /// <summary>
        /// 儲存資料(多筆)
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
        public static bool SaveDatas<T>(this IDataManager<T>? dataManager, IEnumerable<T> datas, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDatas(dataManager?.MainDataSource, datas, dataTransResult, execNow);

        /// <summary>
        /// 儲存資料(多筆)
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
        public static bool SaveDatas<T, U>(this IDataManager<T, U>? dataManager, IEnumerable<U> datas, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDatas(dataManager?.SubDataSource, datas, dataTransResult, execNow);

        /// <summary>
        /// 儲存資料(多筆)
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
        public static bool SaveDatas<T, U, V>(this IDataManager<T, U, V>? dataManager, IEnumerable<V> datas, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDatas(dataManager?.SubDataSource2, datas, dataTransResult, execNow);

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <param name="dataModel">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveData<T>(this IDataManager<T>? dataManager, T? dataModel, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveData(dataManager?.MainDataSource, dataModel, dataTransResult, execNow);

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <param name="dataModel">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveData<T, U>(this IDataManager<T, U>? dataManager, U? dataModel, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveData(dataManager?.SubDataSource, dataModel, dataTransResult, execNow);

        /// <summary>
        /// 移除資料
        /// </summary>
        /// <param name="dataModel">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveData<T, U, V>(this IDataManager<T, U, V>? dataManager, V? dataModel, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveData(dataManager?.SubDataSource2, dataModel, dataTransResult, execNow);

        /// <summary>
        /// 移除符合條件的資料
        /// </summary>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveData<T>(this IDataManager<T>? dataManager, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveDatasBy(dataManager?.MainDataSource, expression, dataTransResult, execNow);

        /// <summary>
        /// 移除符合條件的資料
        /// </summary>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveData<T, U>(this IDataManager<T, U>? dataManager, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveDatasBy(dataManager?.SubDataSource, expression, dataTransResult, execNow);

        /// <summary>
        /// 移除符合條件的資料
        /// </summary>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveData<T, U, V>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveDatasBy(dataManager?.SubDataSource2, expression, dataTransResult, execNow);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型
        /// </summary>
        /// <typeparam name="T">來源資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>轉換後的目的模型</returns>
        public static P? GetDataAs<T, P>(this IDataManager<T>? dataManager, Expression<Func<T, P>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDataAs(dataManager?.MainDataSource, selector, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>轉換後的目的模型</returns>
        public static P? GetDataAs<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<U, P>>? selector, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDataAs(dataManager?.SubDataSource, selector, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>轉換後的目的模型</returns>
        public static P? GetDataAs<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, P>>? selector, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDataAs(dataManager?.SubDataSource2, selector, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型(多筆)
        /// </summary>
        /// <typeparam name="T">來源資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, P>(this IDataManager<T>? dataManager, Expression<Func<T, P>>? selector, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatasAs(dataManager?.MainDataSource, selector, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<U, P>>? selector, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatasAs(dataManager?.SubDataSource, selector, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, P>>? selector, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatasAs(dataManager?.SubDataSource2, selector, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型(指定最大查詢數量)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的主要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, P>(this IDataManager<T>? dataManager, Expression<Func<T, P>>? selector, int begin, int count, Expression<Func<T, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatasAs(dataManager?.MainDataSource, selector, begin, count, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型(指定最大查詢數量)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<U, P>>? selector, int begin, int count, Expression<Func<U, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatasAs(dataManager?.SubDataSource, selector, begin, count, expression, dataTransResult);

        /// <summary>
        /// 取得符合條件的資料並轉型成目的資料模型(指定最大查詢數量)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">目的資料的模型</typeparam>
        /// <param name="selector">轉換到目標模型的敘述式</param>
        /// <param name="begin">開始抓取資料的索引</param>
        /// <param name="count">要抓取的最大筆數</param>
        /// <param name="expression">資料的篩選條件式</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <returns>符合條件的次要資料轉換而成的目的資料模型之集合</returns>
        public static IEnumerable<P> GetDatasAs<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<V, P>>? selector, int begin, int count, Expression<Func<V, bool>>? expression, IDataTransResult? dataTransResult)
            => CheckNullAccesserHelper.GetDatasAs(dataManager?.SubDataSource2, selector, begin, count, expression, dataTransResult);

        /// <summary>
        /// 從外部來源模型更新資料(單筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">來源資料的模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDataFrom<T, P>(this IDataManager<T>? dataManager, Expression<Func<P, T>>? selector, P? data, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDataFrom(dataManager?.MainDataSource, selector, data, dataTransResult, execNow);

        /// <summary>
        /// 從外部來源模型更新資料(單筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">來源資料的模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDataFrom<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<P, U>>? selector, P? data, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDataFrom(dataManager?.SubDataSource, selector, data, dataTransResult, execNow);

        /// <summary>
        /// 從外部來源模型更新資料(單筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">來源資料的模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="data">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDataFrom<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<P, V>>? selector, P? data, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDataFrom(dataManager?.SubDataSource2, selector, data, dataTransResult, execNow);

        /// <summary>
        /// 從外部來源模型更新資料(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="P">輸入資料的模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatasFrom<T, P>(this IDataManager<T>? dataManager, Expression<Func<P, T>>? selector, IEnumerable<P> datas, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDatasFrom(dataManager?.MainDataSource, selector, datas, dataTransResult, execNow);

        /// <summary>
        /// 從外部來源模型更新資料(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型</typeparam>
        /// <typeparam name="P">輸入資料的模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatasFrom<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<P, U>>? selector, IEnumerable<P> datas, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDatasFrom(dataManager?.SubDataSource, selector, datas, dataTransResult, execNow);

        /// <summary>
        /// 從外部來源模型更新資料(多筆)
        /// </summary>
        /// <typeparam name="T">主要資料的模型</typeparam>
        /// <typeparam name="U">次要資料的模型1</typeparam>
        /// <typeparam name="V">次要資料的模型2</typeparam>
        /// <typeparam name="P">輸入資料的模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="datas">來源資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool SaveDatasFrom<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<P, V>>? selector, IEnumerable<P> datas, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.SaveDatasFrom(dataManager?.SubDataSource2, selector, datas, dataTransResult, execNow);

        /// <summary>
        /// 以外部資料模型為對照來移除內部資料
        /// </summary>
        /// <typeparam name="P">外部資料模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="dataModel">外部資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveDataFrom<T, P>(this IDataManager<T>? dataManager, Expression<Func<P, T>>? selector, P? dataModel, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveDataFrom(dataManager?.MainDataSource, selector, dataModel, dataTransResult, execNow);

        /// <summary>
        /// 以外部資料模型為對照來移除內部資料
        /// </summary>
        /// <typeparam name="P">外部資料模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="dataModel">外部資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveDataFrom<T, U, P>(this IDataManager<T, U>? dataManager, Expression<Func<P, U>>? selector, P? dataModel, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveDataFrom(dataManager?.SubDataSource, selector, dataModel, dataTransResult, execNow);

        /// <summary>
        /// 以外部資料模型為對照來移除內部資料
        /// </summary>
        /// <typeparam name="P">外部資料模型</typeparam>
        /// <param name="selector">轉換成內部模型的敘述式</param>
        /// <param name="dataModel">外部資料</param>
        /// <param name="dataTransResult">交易執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>
        /// <para>如不選擇立即執行，正常情況下會回傳 TRUE 以繼續加入資料批次，唯加入過程發生會影響整批交易的失敗，回傳 false 並鎖定整批交易；</para>
        /// <para>如選擇立即執行，回傳執行的結果是否成功；</para>
        /// <para>無論選擇是否立即執行，只要回傳false都代表錯誤發生，故整批交易都會被鎖定</para>
        /// </returns>
        public static bool RemoveDataFrom<T, U, V, P>(this IDataManager<T, U, V>? dataManager, Expression<Func<P, V>>? selector, P? dataModel, IDataTransResult? dataTransResult, bool execNow)
            => CheckNullAccesserHelper.RemoveDataFrom(dataManager?.SubDataSource2, selector, dataModel, dataTransResult, execNow);
    }
}

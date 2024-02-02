using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Cyh.DataHelper;
using Cyh.DataModels;

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
            IEnumerable? dataInputs, IDataTransResult? prevResult = null, bool execNow = true) {
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
        public static bool TryAddOrUpdateSingleT<T>(this IWritableDataAccesser<T> writable,
            object? dataInputs, IDataTransResult? prevResult = null, bool execNow = true) {
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

    public static partial class MyDataHelperExtends {
        /// <summary>
        /// 活性化資料管理器
        /// </summary>
        /// <typeparam name="T">資料的模型</typeparam>
        /// <param name="newMgr">資料管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T>(this IDataManagerActivator? formManager,
            IDataManager<T>? newMgr)
            where T : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainDataSource = formManager.TryGetDataAccesser<T>();

            return newMgr.MainDataSource != null;
        }

        /// <summary>
        /// 活性化資料管理器
        /// </summary>
        /// <typeparam name="T">資料單頭的模型</typeparam>
        /// <typeparam name="U">資料單身的模型</typeparam>
        /// <param name="newMgr">資料管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T, U>(this IDataManagerActivator? formManager,
            IDataManager<T, U>? newMgr)
            where T : class
            where U : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainDataSource = formManager.TryGetDataAccesser<T>();
            newMgr.SubDataSource = formManager.TryGetDataAccesser<U>();

            return newMgr.MainDataSource != null;
        }

        /// <summary>
        /// 活性化資料管理器
        /// </summary>
        /// <typeparam name="T">資料單頭的模型</typeparam>
        /// <typeparam name="U">資料單身的模型1</typeparam>
        /// <typeparam name="V">資料單身的模型2</typeparam>
        /// <param name="newMgr">資料管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T, U, V>(this IDataManagerActivator? formManager,
            IDataManager<T, U, V>? newMgr)
            where T : class
            where U : class
            where V : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainDataSource = formManager.TryGetDataAccesser<T>();
            newMgr.SubDataSource = formManager.TryGetDataAccesser<U>();
            newMgr.SubDataSource2 = formManager.TryGetDataAccesser<V>();

            return newMgr.MainDataSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="TForm">資料的模型</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<TForm>([NotNullWhen(true)] this IDataManager<TForm>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainDataSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<MF, TF>([NotNullWhen(true)] this IDataManager<MF, TF>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainDataSource != null && formMgr.SubDataSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型1</typeparam>
        /// <typeparam name="TF2">資料單身的模型2</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<MF, TF1, TF2>([NotNullWhen(true)] this IDataManager<MF, TF1, TF2>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainDataSource != null && formMgr.SubDataSource != null && formMgr.SubDataSource2 != null;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <typeparam name="TForm">資料的模型</typeparam>
        /// <param name="expression"></param>
        /// <returns>符合條件的資料</returns>
        public static TForm? GetMainForm<TForm>(this IDataManager<TForm>? formManager,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formManager.MainDataSource.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單頭
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單頭</returns>
        public static MF? GetMainForm<MF, TF>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.MainDataSource.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單身
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單身</returns>
        public static TF? GetTargetForm<MF, TF>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<TF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.SubDataSource.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單身2
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型1</typeparam>
        /// <typeparam name="TF2">資料單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單身2</returns>
        public static TF2? GetTargetForm2<MF, TF1, TF2>(this IDataManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.SubDataSource2.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單頭的集合
        /// </summary>
        /// <typeparam name="MF">資料的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料集合</returns>
        public static IEnumerable<MF> GetMainForms<MF>(this IDataManager<MF>? formMgr,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MF>();
            return formMgr.MainDataSource?.TryGetDatas(expression) ?? Enumerable.Empty<MF>();
        }

        /// <summary>
        /// 取得資料單頭的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單頭的集合</returns>
        public static IEnumerable<MF> GetMainForms<MF, TF>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MF>();
            return formMgr.MainDataSource?.TryGetDatas(expression) ?? Enumerable.Empty<MF>();
        }

        /// <summary>
        /// 取得資料單身的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單身的集合</returns>
        public static IEnumerable<TF> GetSubForms<MF, TF>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<TF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF>();
            return formMgr.SubDataSource?.TryGetDatas(expression) ?? Enumerable.Empty<TF>();
        }

        /// <summary>
        /// 取得資料單身2集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型1</typeparam>
        /// <typeparam name="TF2">資料單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單身2的集合</returns>
        public static IEnumerable<TF2> GetTargetForm2s<MF, TF1, TF2>(this IDataManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2>();
            return formMgr.SubDataSource2?.TryGetDatas(expression) ?? Enumerable.Empty<TF2>();
        }

        /// <summary>
        /// 取得資料單頭的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單頭的集合</returns>
        public static IEnumerable<MF> GetMainForms<MF, TF>(this IDataManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MF>();
            return formMgr.MainDataSource?.TryGetDatas(begin, count, expression) ?? Enumerable.Empty<MF>();
        }

        /// <summary>
        /// 取得資料單身的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單身的集合</returns>
        public static IEnumerable<TF> GetSubForms<MF, TF>(this IDataManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF>();
            return formMgr.SubDataSource?.TryGetDatas(begin, count, expression) ?? Enumerable.Empty<TF>();
        }

        /// <summary>
        /// 取得資料單身2集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型1</typeparam>
        /// <typeparam name="TF2">資料單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的資料單身2的集合</returns>
        public static IEnumerable<TF2> GetTargetForm2s<MF, TF1, TF2>(this IDataManager<MF, TF1, TF2>? formMgr, int begin, int count,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2>();
            return formMgr.SubDataSource2?.TryGetDatas(begin, count, expression) ?? Enumerable.Empty<TF2>();
        }
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 取得資料相關的資料模型
        /// </summary>
        /// <typeparam name="TForm">資料的模型</typeparam>
        /// <typeparam name="TOut">資料相關的資料模型</typeparam>
        /// <param name="selector">從資料模型到資料相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料相關的資料模型</returns>
        public static TOut? GetMainFormAs<TForm, TOut>(this IDataManager<TForm>? formManager,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formManager.MainDataSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料相關的資料模型的集合
        /// </summary>
        /// <typeparam name="TForm">資料的模型</typeparam>
        /// <typeparam name="TOut">資料相關的資料模型</typeparam>
        /// <param name="selector">從資料模型到資料相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料相關的資料模型的集合</returns>
        public static IEnumerable<TOut> GetMainFormsAs<TForm, TOut>(this IDataManager<TForm>? formManager,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return Enumerable.Empty<TOut>();
#pragma warning disable CS8604
            return formManager.MainDataSource.TryGetDatasAs(selector, expression);
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單頭相關的資料模型
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="MFOut">資料單頭資相關的資料模型</typeparam>
        /// <param name="selector">從資料單頭模型到資料單頭資料相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單頭相關的資料模型</returns>
        public static MFOut? GetMainFormAs<MF, TF, MFOut>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.MainDataSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單身相關的資料模型
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="TFOut">資料單身資相關的資料模型</typeparam>
        /// <param name="selector">從資料單身模型到資料單身相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單身相關的資料模型</returns>
        public static TFOut? GetTargetFormAs<MF, TF, TFOut>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.SubDataSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單身2相關的資料模型
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型</typeparam>
        /// <typeparam name="TF2">資料單身2的模型</typeparam>
        /// <typeparam name="TF2Out">資料單身2資相關的資料模型</typeparam>
        /// <param name="selector">從資料單身模型2到資料單身2相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單身2相關的資料模型</returns>
        public static TF2Out? GetTargetForm2As<MF, TF1, TF2, TF2Out>(this IDataManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.SubDataSource2.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單頭相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="MFOut">資料單頭資相關的資料模型</typeparam>
        /// <param name="selector">從資料單頭模型到資料單頭資相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單頭相關的資料模型的集合</returns>
        public static IEnumerable<MFOut> GetMainFormsAs<MF, TF, MFOut>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MFOut>();
            return formMgr.MainDataSource?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<MFOut>();
        }

        /// <summary>
        /// 取得資料單身相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="TFOut">資料單身資相關的資料模型</typeparam>
        /// <param name="selector">從資料單身模型到資料單身相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單身相關的資料模型的集合</returns>
        public static IEnumerable<TFOut> GetSubFormsAs<MF, TF, TFOut>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TFOut>();
            return formMgr.SubDataSource?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<TFOut>();
        }

        /// <summary>
        /// 取得資料單身2相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型</typeparam>
        /// <typeparam name="TF2">資料單身2的模型</typeparam>
        /// <typeparam name="TF2Out">資料單身2資相關的資料模型</typeparam>
        /// <param name="selector">從資料單身模型2到資料單身2相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單身2相關的資料模型的集合</returns>
        public static IEnumerable<TF2Out> GetTargetForm2sAs<MF, TF1, TF2, TF2Out>(this IDataManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2Out>();
            return formMgr.SubDataSource2?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<TF2Out>();
        }

        /// <summary>
        /// 取得資料相關的資料模型的集合
        /// </summary>
        /// <typeparam name="TForm">資料的模型</typeparam>
        /// <typeparam name="TOut">資料相關的資料模型</typeparam>
        /// <param name="selector">從資料模型到資料相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料相關的資料模型的集合</returns>
        public static IEnumerable<TOut> GetMainFormsAs<TForm, TOut>(this IDataManager<TForm>? formManager, int begin, int count,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression = null) {
            if (!formManager.SourceIsValid())
                return Enumerable.Empty<TOut>();
#pragma warning disable CS8604
            return formManager.MainDataSource.TryGetDatasAs(selector, begin, count, expression);
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得資料單頭相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="MFOut">資料單頭資相關的資料模型</typeparam>
        /// <param name="selector">從資料單頭模型到資料單頭資相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單頭相關的資料模型的集合</returns>
        public static IEnumerable<MFOut> GetMainFormsAs<MF, TF, MFOut>(this IDataManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MFOut>();
            return formMgr.MainDataSource?.TryGetDatasAs(selector, begin, count, expression) ?? Enumerable.Empty<MFOut>();
        }

        /// <summary>
        /// 取得資料單身相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="TFOut">資料單身資相關的資料模型</typeparam>
        /// <param name="selector">從資料單身模型到資料單身相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單身相關的資料模型的集合</returns>
        public static IEnumerable<TFOut> GetSubFormsAs<MF, TF, TFOut>(this IDataManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TFOut>();
            return formMgr.SubDataSource?.TryGetDatasAs(selector, begin, count, expression) ?? Enumerable.Empty<TFOut>();
        }

        /// <summary>
        /// 取得資料單身2相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF1">資料單身的模型</typeparam>
        /// <typeparam name="TF2">資料單身2的模型</typeparam>
        /// <typeparam name="TF2Out">資料單身2資相關的資料模型</typeparam>
        /// <param name="selector">從資料單身模型2到資料單身2相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>資料單身2相關的資料模型的集合</returns>
        public static IEnumerable<TF2Out> GetTargetForm2sAs<MF, TF1, TF2, TF2Out>(this IDataManager<MF, TF1, TF2>? formMgr, int begin, int count,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2Out>();
            return formMgr.SubDataSource2?.TryGetDatasAs(selector, begin, count, expression) ?? Enumerable.Empty<TF2Out>();
        }
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 儲存資料單頭
        /// </summary>
        /// <typeparam name="TForm">資料單頭的模型</typeparam>
        /// <param name="form">資料單頭</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>是否成功</returns>
        public static bool SaveMainForm<TForm>(this IDataManager<TForm>? formManager,
        TForm? form, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || form == null)
                return false;
            return formManager.MainDataSource?.TryAddOrUpdate(form, prevResult, execNow) ?? false;
        }

        /// <summary>
        /// 儲存資料單身
        /// </summary>
        /// <typeparam name="TMainForm">資料單頭的模型</typeparam>
        /// <typeparam name="TTargetForm">資料單身的模型</typeparam>
        /// <param name="form">資料單身</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>是否成功</returns>
        public static bool SaveTargetForm<TMainForm, TTargetForm>(this IDataManager<TMainForm, TTargetForm>? formManager,
            TTargetForm? form, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || form == null)
                return false;
            return formManager.SubDataSource?.TryAddOrUpdate(form, prevResult, execNow) ?? false;
        }

        /// <summary>
        /// 儲存資料單頭集合
        /// </summary>
        /// <typeparam name="TForm">資料單頭的模型</typeparam>
        /// <param name="forms">資料單頭集合</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveMainForms<TForm>(this IDataManager<TForm>? formManager,
            IEnumerable<TForm> forms, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || forms.IsNullOrEmpty())
                return default;
            return formManager.MainDataSource?.TryAddOrUpdate(forms, prevResult, execNow);
        }

        /// <summary>
        /// 儲存資料單身集合
        /// </summary>
        /// <typeparam name="TMainForm">資料單頭的模型</typeparam>
        /// <typeparam name="TTargetForm">資料單身的模型</typeparam>
        /// <param name="forms">資料單身集合</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveSubForms<TMainForm, TTargetForm>(this IDataManager<TMainForm, TTargetForm>? formManager,
            IEnumerable<TTargetForm>? forms, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || forms.IsNullOrEmpty())
                return default;
            return formManager.SubDataSource?.TryAddOrUpdate(forms, prevResult, execNow);
        }
    }

    public static partial class MyDataHelperExtends
    {
        /// <summary>
        /// 儲存資料單頭
        /// </summary>
        /// <typeparam name="TForm">資料單頭的模型</typeparam>
        /// <param name="form">資料單頭</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>是否成功</returns>
        public static bool SaveMainForm<TForm, TDataIn>(this IDataManager<TForm>? formManager,
            Expression<Func<TDataIn, TForm>>? expression,
            TDataIn? form, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || form == null || expression == null)
                return false;

            try {
                var program = expression.Compile();
                return formManager.MainDataSource?.TryAddOrUpdate(program(form), prevResult, execNow) ?? false;
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// 儲存資料單身
        /// </summary>
        /// <typeparam name="TMainForm">資料單頭的模型</typeparam>
        /// <typeparam name="TTargetForm">資料單身的模型</typeparam>
        /// <param name="form">資料單身</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>是否成功</returns>
        public static bool SaveTargetForm<TMainForm, TTargetForm, TTargetDataIn>(this IDataManager<TMainForm, TTargetForm>? formManager,
            Expression<Func<TTargetDataIn, TTargetForm>>? expression,
            TTargetDataIn? form, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || form == null || expression == null)
                return false;
            try {
                var program = expression.Compile();
                return formManager.SubDataSource?.TryAddOrUpdate(program(form), prevResult, execNow) ?? false;
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// 儲存資料單頭集合
        /// </summary>
        /// <typeparam name="TForm">資料單頭的模型</typeparam>
        /// <param name="forms">資料單頭集合</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveMainForms<TForm, TDataIn>(this IDataManager<TForm>? formManager,
            Expression<Func<TDataIn, TForm>>? expression,
            IEnumerable<TDataIn> forms, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || forms.IsNullOrEmpty() || expression == null)
                return formManager?.MainDataSource?.EmptyResult;
            IDataTransResult? result = prevResult ?? formManager?.MainDataSource?.EmptyResult;
            if (result == null)
                return result;

            Func<TDataIn, TForm>? program = null;
            try {
                program = expression.Compile();
                if (program == null)
                    return result;
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
                return result;
            }

            try {
                IEnumerable<TForm> data = forms.Select(program);
                return formManager?.MainDataSource?.TryAddOrUpdate(data, prevResult, execNow);
            } catch (Exception ex) {
                result.BatchOnFinish(false);
                CommonLib.HandleException(ex);
                return result;
            }
        }

        /// <summary>
        /// 儲存資料單頭集合
        /// </summary>
        /// <typeparam name="TForm">資料單頭的模型</typeparam>
        /// <param name="forms">資料單頭集合</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveMainFormsAs<TForm, TDataIn>(this IDataManager<TForm>? formManager,
            Expression<Func<TDataIn, TForm>>? expression,
            IEnumerable<TDataIn> forms, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || forms.IsNullOrEmpty() || expression == null)
                return formManager?.MainDataSource?.EmptyResult;
            IDataTransResult? result = prevResult ?? formManager?.MainDataSource?.EmptyResult;
            if (result == null)
                return result;

            Func<TDataIn, TForm>? program = null;
            try {
                program = expression.Compile();
                if (program == null)
                    return result;
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
                return result;
            }

            try {
                IEnumerable<TForm> data = forms.Select(program);
                return formManager?.MainDataSource?.TryAddOrUpdate(data, prevResult, execNow);
            } catch (Exception ex) {
                result.BatchOnFinish(false);
                CommonLib.HandleException(ex);
                return result;
            }
        }

        /// <summary>
        /// 儲存資料單身集合
        /// </summary>
        /// <typeparam name="TMainForm">資料單頭的模型</typeparam>
        /// <typeparam name="TTargetForm">資料單身的模型</typeparam>
        /// <param name="tforms">資料單身集合</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveSubForms<TMainForm, TTargetForm, TTargetDataIn>(this IDataManager<TMainForm, TTargetForm>? formManager,
            Expression<Func<TTargetDataIn, TTargetForm>>? expression,
            IEnumerable<TTargetDataIn> tforms, IDataTransResult? prevResult = null, bool execNow = true) {
            if (!formManager.SourceIsValid() || tforms.IsNullOrEmpty() || expression == null)
                return formManager?.SubDataSource?.EmptyResult;
            IDataTransResult? result = prevResult ?? formManager?.MainDataSource?.EmptyResult;
            if (result == null)
                return result;
            Func<TTargetDataIn, TTargetForm>? program;
            try {
                program = expression.Compile();
                if (program == null)
                    return result;
            } catch (Exception ex) {
                CommonLib.HandleException(ex);
                return result;
            }
            try {
                IEnumerable<TTargetForm> data = tforms.Select(program);
                return formManager?.SubDataSource?.TryAddOrUpdate(data, prevResult, execNow);
            } catch (Exception ex) {
                result.BatchOnFinish(false);
                CommonLib.HandleException(ex);
                return result;
            }
        }
    };
}

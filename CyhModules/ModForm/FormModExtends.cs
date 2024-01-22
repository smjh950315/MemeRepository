using Cyh.DataHelper;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Cyh.Modules.ModForm
{
    public static partial class FormModExtends
    {
        /// <summary>
        /// 活性化表單管理器
        /// </summary>
        /// <typeparam name="T">表單的模型</typeparam>
        /// <param name="newMgr">表單管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T>(this IFormManagerActivator? formManager,
            IFormManager<T>? newMgr)
            where T : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainFormSource = formManager.TryGetDataAccesser<T>();

            return newMgr.MainFormSource != null;
        }

        /// <summary>
        /// 活性化表單管理器
        /// </summary>
        /// <typeparam name="T">表單單頭的模型</typeparam>
        /// <typeparam name="U">表單單身的模型</typeparam>
        /// <param name="newMgr">表單管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T, U>(this IFormManagerActivator? formManager,
            IFormManager<T, U>? newMgr)
            where T : class
            where U : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainFormSource = formManager.TryGetDataAccesser<T>();
            newMgr.TargetFormSource = formManager.TryGetDataAccesser<U>();

            return newMgr.MainFormSource != null;
        }

        /// <summary>
        /// 活性化表單管理器
        /// </summary>
        /// <typeparam name="T">表單單頭的模型</typeparam>
        /// <typeparam name="U">表單單身的模型1</typeparam>
        /// <typeparam name="V">表單單身的模型2</typeparam>
        /// <param name="newMgr">表單管理器的實體</param>
        /// <returns>是否成功</returns>
        public static bool Activate<T, U, V>(this IFormManagerActivator? formManager,
            IFormManager<T, U, V>? newMgr)
            where T : class
            where U : class
            where V : class {

            if (formManager == null || newMgr == null)
                return false;

            newMgr.MainFormSource = formManager.TryGetDataAccesser<T>();
            newMgr.TargetFormSource = formManager.TryGetDataAccesser<U>();
            newMgr.TargetFormSource2 = formManager.TryGetDataAccesser<V>();

            return newMgr.MainFormSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="TForm">表單的模型</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<TForm>([NotNullWhen(true)] this IFormManager<TForm>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainFormSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<MF, TF>([NotNullWhen(true)] this IFormManager<MF, TF>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainFormSource != null && formMgr.TargetFormSource != null;
        }

        /// <summary>
        /// 資料源是否為有效狀態
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型1</typeparam>
        /// <typeparam name="TF2">表單單身的模型2</typeparam>
        /// <returns>是否為有效狀態</returns>
        public static bool SourceIsValid<MF, TF1, TF2>([NotNullWhen(true)] this IFormManager<MF, TF1, TF2>? formMgr) {
            if (formMgr == null)
                return false;
            return formMgr.MainFormSource != null && formMgr.TargetFormSource != null && formMgr.TargetFormSource2 != null;
        }

        /// <summary>
        /// 取得表單
        /// </summary>
        /// <typeparam name="TForm">表單的模型</typeparam>
        /// <param name="expression"></param>
        /// <returns>符合條件的表單</returns>
        public static TForm? GetMainForm<TForm>(this IFormManager<TForm>? formManager,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formManager.MainFormSource.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單頭
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單頭</returns>
        public static MF? GetMainForm<MF, TF>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.MainFormSource.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單身
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單身</returns>
        public static TF? GetTargetForm<MF, TF>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<TF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.TargetFormSource.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單身2
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型1</typeparam>
        /// <typeparam name="TF2">表單單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單身2</returns>
        public static TF2? GetTargetForm2<MF, TF1, TF2>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.TargetFormSource2.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單頭的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單頭的集合</returns>
        public static IEnumerable<MF> GetMainForms<MF, TF>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MF>();
            return formMgr.MainFormSource?.TryGetDatas(expression) ?? Enumerable.Empty<MF>();
        }

        /// <summary>
        /// 取得表單單身的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單身的集合</returns>
        public static IEnumerable<TF> GetTargetForms<MF, TF>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<TF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF>();
            return formMgr.TargetFormSource?.TryGetDatas(expression) ?? Enumerable.Empty<TF>();
        }

        /// <summary>
        /// 取得表單單身2集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型1</typeparam>
        /// <typeparam name="TF2">表單單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單身2的集合</returns>
        public static IEnumerable<TF2> GetTargetForm2s<MF, TF1, TF2>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2>();
            return formMgr.TargetFormSource2?.TryGetDatas(expression) ?? Enumerable.Empty<TF2>();
        }

        /// <summary>
        /// 取得表單單頭的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單頭的集合</returns>
        public static IEnumerable<MF> GetMainForms<MF, TF>(this IFormManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MF>();
            return formMgr.MainFormSource?.TryGetDatas(begin, count, expression) ?? Enumerable.Empty<MF>();
        }

        /// <summary>
        /// 取得表單單身的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單身的集合</returns>
        public static IEnumerable<TF> GetTargetForms<MF, TF>(this IFormManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<TF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF>();
            return formMgr.TargetFormSource?.TryGetDatas(begin, count, expression) ?? Enumerable.Empty<TF>();
        }

        /// <summary>
        /// 取得表單單身2集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型1</typeparam>
        /// <typeparam name="TF2">表單單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>符合條件的表單單身2的集合</returns>
        public static IEnumerable<TF2> GetTargetForm2s<MF, TF1, TF2>(this IFormManager<MF, TF1, TF2>? formMgr, int begin, int count,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2>();
            return formMgr.TargetFormSource2?.TryGetDatas(begin, count, expression) ?? Enumerable.Empty<TF2>();
        }
    }

    public static partial class FormModExtends
    {
        /// <summary>
        /// 取得表單相關的資料模型
        /// </summary>
        /// <typeparam name="TForm">表單的模型</typeparam>
        /// <typeparam name="TOut">表單相關的資料模型</typeparam>
        /// <param name="selector">從表單模型到表單相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單相關的資料模型</returns>
        public static TOut? GetMainFormAs<TForm, TOut>(this IFormManager<TForm>? formManager,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formManager.MainFormSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單相關的資料模型的集合
        /// </summary>
        /// <typeparam name="TForm">表單的模型</typeparam>
        /// <typeparam name="TOut">表單相關的資料模型</typeparam>
        /// <param name="selector">從表單模型到表單相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單相關的資料模型的集合</returns>
        public static IEnumerable<TOut> GetMainFormsAs<TForm, TOut>(this IFormManager<TForm>? formManager,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return Enumerable.Empty<TOut>();
#pragma warning disable CS8604
            return formManager.MainFormSource.TryGetDatasAs(selector, expression);
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單頭相關的資料模型
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="MFOut">表單單頭資相關的資料模型</typeparam>
        /// <param name="selector">從表單單頭模型到表單單頭資相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單頭相關的資料模型</returns>
        public static MFOut? GetMainFormAs<MF, TF, MFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.MainFormSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單身相關的資料模型
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="TFOut">表單單身資相關的資料模型</typeparam>
        /// <param name="selector">從表單單身模型到表單單身相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身相關的資料模型</returns>
        public static TFOut? GetTargetFormAs<MF, TF, TFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.TargetFormSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單身2相關的資料模型
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型</typeparam>
        /// <typeparam name="TF2">表單單身2的模型</typeparam>
        /// <typeparam name="TF2Out">表單單身2資相關的資料模型</typeparam>
        /// <param name="selector">從表單單身模型2到表單單身2相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身2相關的資料模型</returns>
        public static TF2Out? GetTargetForm2As<MF, TF1, TF2, TF2Out>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.TargetFormSource2.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單頭相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="MFOut">表單單頭資相關的資料模型</typeparam>
        /// <param name="selector">從表單單頭模型到表單單頭資相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單頭相關的資料模型的集合</returns>
        public static IEnumerable<MFOut> GetMainFormsAs<MF, TF, MFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MFOut>();
            return formMgr.MainFormSource?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<MFOut>();
        }

        /// <summary>
        /// 取得表單單身相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="TFOut">表單單身資相關的資料模型</typeparam>
        /// <param name="selector">從表單單身模型到表單單身相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身相關的資料模型的集合</returns>
        public static IEnumerable<TFOut> GetTargetFormsAs<MF, TF, TFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TFOut>();
            return formMgr.TargetFormSource?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<TFOut>();
        }

        /// <summary>
        /// 取得表單單身2相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型</typeparam>
        /// <typeparam name="TF2">表單單身2的模型</typeparam>
        /// <typeparam name="TF2Out">表單單身2資相關的資料模型</typeparam>
        /// <param name="selector">從表單單身模型2到表單單身2相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身2相關的資料模型的集合</returns>
        public static IEnumerable<TF2Out> GetTargetForm2sAs<MF, TF1, TF2, TF2Out>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2Out>();
            return formMgr.TargetFormSource2?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<TF2Out>();
        }
        
        /// <summary>
        /// 取得表單相關的資料模型的集合
        /// </summary>
        /// <typeparam name="TForm">表單的模型</typeparam>
        /// <typeparam name="TOut">表單相關的資料模型</typeparam>
        /// <param name="selector">從表單模型到表單相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單相關的資料模型的集合</returns>
        public static IEnumerable<TOut> GetMainFormsAs<TForm, TOut>(this IFormManager<TForm>? formManager, int begin, int count,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return Enumerable.Empty<TOut>();
#pragma warning disable CS8604
            return formManager.MainFormSource.TryGetDatasAs(selector, begin, count, expression);
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單頭相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="MFOut">表單單頭資相關的資料模型</typeparam>
        /// <param name="selector">從表單單頭模型到表單單頭資相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單頭相關的資料模型的集合</returns>
        public static IEnumerable<MFOut> GetMainFormsAs<MF, TF, MFOut>(this IFormManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MFOut>();
            return formMgr.MainFormSource?.TryGetDatasAs(selector, begin, count, expression) ?? Enumerable.Empty<MFOut>();
        }

        /// <summary>
        /// 取得表單單身相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="TFOut">表單單身資相關的資料模型</typeparam>
        /// <param name="selector">從表單單身模型到表單單身相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身相關的資料模型的集合</returns>
        public static IEnumerable<TFOut> GetTargetFormsAs<MF, TF, TFOut>(this IFormManager<MF, TF>? formMgr, int begin, int count,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TFOut>();
            return formMgr.TargetFormSource?.TryGetDatasAs(selector, begin, count, expression) ?? Enumerable.Empty<TFOut>();
        }

        /// <summary>
        /// 取得表單單身2相關的資料模型的集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型</typeparam>
        /// <typeparam name="TF2">表單單身2的模型</typeparam>
        /// <typeparam name="TF2Out">表單單身2資相關的資料模型</typeparam>
        /// <param name="selector">從表單單身模型2到表單單身2相關的資料模型的 selector</param>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身2相關的資料模型的集合</returns>
        public static IEnumerable<TF2Out> GetTargetForm2sAs<MF, TF1, TF2, TF2Out>(this IFormManager<MF, TF1, TF2>? formMgr, int begin, int count,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2Out>();
            return formMgr.TargetFormSource2?.TryGetDatasAs(selector, begin, count, expression) ?? Enumerable.Empty<TF2Out>();
        }
    }

    public static partial class FormModExtends
    {
        /// <summary>
        /// 取得表單資料
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="TFormGroup">表單</typeparam>
        /// <param name="expression">取得單頭的條件</param>
        /// <param name="expression2">取得單身的條件</param>
        /// <returns>表單資料</returns>
        public static TFormGroup? GetFormGroup<MF, TF, TFormGroup>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>> expression, Expression<Func<TF, bool>> expression2)
            where TFormGroup : class, IFormGroup<MF, TF>, new() {

            if (formMgr == null)
                return null;
            if (!formMgr.SourceIsValid())
                return null;

            TFormGroup formGroup = new TFormGroup();

            formGroup.MainForm = formMgr.GetMainForm(expression);
            formGroup.TargetForms = formMgr.GetTargetForms(expression2);

            return formGroup;
        }
    }

    public static partial class FormModExtends
    {
        /// <summary>
        /// 儲存表單單頭
        /// </summary>
        /// <typeparam name="TForm">表單單頭的模型</typeparam>
        /// <param name="form">表單單頭</param>
        /// <returns>是否成功</returns>
        public static bool SaveMainForm<TForm>(this IFormManager<TForm>? formManager,
        TForm? form) {
            if (!formManager.SourceIsValid() || form == null)
                return false;
            return formManager.MainFormSource?.TryAddOrUpdate(form) ?? false;
        }

        /// <summary>
        /// 儲存表單單身
        /// </summary>
        /// <typeparam name="TMainForm">表單單頭的模型</typeparam>
        /// <typeparam name="TTargetForm">表單單身的模型</typeparam>
        /// <param name="form">表單單身</param>
        /// <returns>是否成功</returns>
        public static bool SaveTargetForm<TMainForm, TTargetForm>(this IFormManager<TMainForm, TTargetForm>? formManager,
            TTargetForm? form) {
            if (!formManager.SourceIsValid() || form == null)
                return false;
            return formManager.TargetFormSource?.TryAddOrUpdate(form) ?? false;
        }

        /// <summary>
        /// 儲存表單單頭集合
        /// </summary>
        /// <typeparam name="TForm">表單單頭的模型</typeparam>
        /// <param name="forms">表單單頭集合</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveMainForms<TForm>(this IFormManager<TForm>? formManager,
            IEnumerable<TForm> forms) {
            if (!formManager.SourceIsValid() || forms.IsNullOrEmpty())
                return default;
            return formManager.MainFormSource?.TryAddOrUpdate(forms);
        }

        /// <summary>
        /// 儲存表單單身集合
        /// </summary>
        /// <typeparam name="TMainForm">表單單頭的模型</typeparam>
        /// <typeparam name="TTargetForm">表單單身的模型</typeparam>
        /// <param name="forms">表單單身集合</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveTargetForms<TMainForm, TTargetForm>(this IFormManager<TMainForm, TTargetForm>? formManager,
            IEnumerable<TTargetForm>? forms) {
            if (!formManager.SourceIsValid() || forms.IsNullOrEmpty())
                return default;
            return formManager.TargetFormSource?.TryAddOrUpdate(forms);
        }
    }

    public static partial class FormModExtends
    {
        /// <summary>
        /// 取得表單資料
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <typeparam name="TFormGroup">表單的模型</typeparam>
        /// <param name="formMgr">表單管理器</param>
        /// <param name="formGroup">要儲存的表單</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveFormGroup<MF, TF, TFormGroup>(this IFormManager<MF, TF>? formMgr,
            TFormGroup formGroup)
            where TFormGroup : class, IFormGroup<MF, TF>, new() {

            if (formMgr == null)
                return default;
            if (!formMgr.SourceIsValid())
                return default;

            if (!formMgr.SaveMainForm(formGroup.MainForm))
                return default;

            return formMgr.SaveTargetForms(formGroup.TargetForms);
        }
    }
}

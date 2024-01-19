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
        /// <typeparam name="FM">表單管理器的類型</typeparam>
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
        /// <typeparam name="FM">表單管理器的類型</typeparam>
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
        /// <typeparam name="FM">表單管理器的類型</typeparam>
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
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
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
        /// <returns>表單單頭</returns>
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
        /// <returns>表單單身</returns>
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
        /// <returns>表單單身2</returns>
        public static TF2? GetTargetForm2<MF, TF1, TF2>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.TargetFormSource2.TryGetData(expression) ?? default;
#pragma warning restore CS8604
        }

        /// <summary>
        /// 取得表單單頭
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單頭的集合</returns>
        public static IEnumerable<MF> GetMainForms<MF, TF>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MF>();
            return formMgr.MainFormSource?.TryGetDatas(expression) ?? Enumerable.Empty<MF>();
        }

        /// <summary>
        /// 取得表單單頭
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身的集合</returns>
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
        /// <returns>表單單身2的集合</returns>
        public static IEnumerable<TF2> GetTargetForm2s<MF, TF1, TF2>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, bool>>? expression) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2>();
            return formMgr.TargetFormSource2?.TryGetDatas(expression) ?? Enumerable.Empty<TF2>();
        }

    }

    public static partial class FormModExtends
    {
        public static TOut? GetMainFormAs<TForm,TOut>(this IFormManager<TForm>? formManager,
            Expression<Func<TForm, TOut>>? selector,
            Expression<Func<TForm, bool>>? expression) {
            if (!formManager.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formManager.MainFormSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }


        /// <summary>
        /// 取得表單單頭
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單頭</returns>
        public static MFOut? GetMainFormAs<MF, TF, MFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return default;
#pragma warning disable CS8604
            return formMgr.MainFormSource.TryGetDataAs(selector, expression) ?? default;
#pragma warning restore CS8604
        }

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
        /// 取得表單單身
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身</returns>
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
        /// 取得表單單身2
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型1</typeparam>
        /// <typeparam name="TF2">表單單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身2</returns>
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
        /// 取得表單單頭
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單頭的集合</returns>
        public static IEnumerable<MFOut> GetMainFormsAs<MF, TF, MFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<MF, MFOut>>? selector,
            Expression<Func<MF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<MFOut>();
            return formMgr.MainFormSource?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<MFOut>();
        }

        /// <summary>
        /// 取得表單單頭
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF">表單單身的模型</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身的集合</returns>
        public static IEnumerable<TFOut> GetTargetFormsAs<MF, TF, TFOut>(this IFormManager<MF, TF>? formMgr,
            Expression<Func<TF, TFOut>>? selector,
            Expression<Func<TF, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TFOut>();
            return formMgr.TargetFormSource?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<TFOut>();
        }

        /// <summary>
        /// 取得表單單身2集合
        /// </summary>
        /// <typeparam name="MF">表單單頭的模型</typeparam>
        /// <typeparam name="TF1">表單單身的模型1</typeparam>
        /// <typeparam name="TF2">表單單身的模型2</typeparam>
        /// <param name="expression">條件表達式</param>
        /// <returns>表單單身2的集合</returns>
        public static IEnumerable<TF2Out> GetTargetForm2sAs<MF, TF1, TF2, TF2Out>(this IFormManager<MF, TF1, TF2>? formMgr,
            Expression<Func<TF2, TF2Out>>? selector,
            Expression<Func<TF2, bool>>? expression = null) {
            if (!formMgr.SourceIsValid())
                return Enumerable.Empty<TF2Out>();
            return formMgr.TargetFormSource2?.TryGetDatasAs(selector, expression) ?? Enumerable.Empty<TF2Out>();
        }

    }

    public static partial class FormModExtends
    {
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

}

using Cyh.DataHelper;
using Cyh.DataModels;
using System.Linq.Expressions;

namespace Cyh.Modules.ModForm
{
    public static partial class FormModExtends
    {
        /// <summary>
        /// 取得資料資料
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="TFormGroup">資料</typeparam>
        /// <param name="expression">取得單頭的條件</param>
        /// <param name="expression2">取得單身的條件</param>
        /// <returns>資料資料</returns>
        public static TFormGroup? GetFormGroup<MF, TF, TFormGroup>(this IDataManager<MF, TF>? formMgr,
            Expression<Func<MF, bool>> expression, Expression<Func<TF, bool>> expression2)
            where TFormGroup : class, IFormGroup<MF, TF>, new() {

            if (formMgr == null)
                return null;
            if (!formMgr.SourceIsValid())
                return null;

            TFormGroup formGroup = new TFormGroup
            {
                MainForm = formMgr.GetMainForm(expression),
                SubForms = formMgr.GetSubForms(expression2)
            };

            return formGroup;
        }

        /// <summary>
        /// 儲存資料資料
        /// </summary>
        /// <typeparam name="MF">資料單頭的模型</typeparam>
        /// <typeparam name="TF">資料單身的模型</typeparam>
        /// <typeparam name="TFormGroup">資料的模型</typeparam>
        /// <param name="formMgr">資料管理器</param>
        /// <param name="formGroup">要儲存的資料</param>
        /// <param name="prevResult">前一批執行結果</param>
        /// <param name="execNow">是否立即執行</param>
        /// <returns>資料交易的結果</returns>
        public static IDataTransResult? SaveFormGroup<MF, TF, TFormGroup>(this IDataManager<MF, TF>? formMgr,
            TFormGroup formGroup, IDataTransResult? prevResult = null, bool execNow = true)
            where TFormGroup : class, IFormGroup<MF, TF>, new() {

            if (formMgr == null)
                return default;
            if (!formMgr.SourceIsValid())
                return default;

            if (!formMgr.SaveMainForm(formGroup.MainForm, prevResult, execNow))
                return default;

            return formMgr.SaveSubForms(formGroup.SubForms, prevResult, execNow);
        }
    }
}

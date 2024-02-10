using Cyh.DataHelper;
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
                MainForm = formMgr.GetData(expression, null),
                SubForms = formMgr.GetDatas(expression2, null)
            };

            return formGroup;
        }
    }
}

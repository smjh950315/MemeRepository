using System.Linq.Expressions;

namespace Cyh.DataHelper
{
    public interface IRelatedModelHelper<TMainModel, TSubModel>
    {
        /// <summary>
        /// 用來查詢與輸入主資料相關的副資料模型取得函數
        /// </summary>
        /// <param name="data">主資料模型</param>
        /// <returns>查詢用的敘述式</returns>
        protected abstract Expression<Func<TSubModel, bool>> GetExprRelateToMainData(TMainModel data);
    }
}

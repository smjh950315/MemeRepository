using Cyh.Common;

namespace Cyh.Modules.ModAuthentication
{
    /// <summary>
    /// 認證用的 Model
    /// </summary>
    public interface IAuthorizationModel
    {
        /// <summary>
        /// 取得準備認證用的資料集
        /// </summary>
        /// <returns>準備認證用的資料集</returns>
        MyDataSet GetValidationDataSet();
    }
}

using Cyh.DataModels;

namespace Cyh.DataHelper
{
    /// <summary>
    /// 資料驗證器
    /// </summary>
    public interface IMyDataValidator
    {
        /// <summary>
        /// 驗證輸入物件是否符合要求
        /// </summary>
        /// <param name="value"></param>
        /// <returns>輸入是否符合要求</returns>
        bool IsValid(IValidationData value);
    }

    /// <summary>
    /// 資料驗證器
    /// </summary>
    public interface IDataValidator<T> where T : IMyDataValidator
    {
        /// <summary>
        /// 驗證輸入物件是否符合要求
        /// </summary>
        /// <param name="value"></param>
        /// <returns>輸入是否符合要求</returns>
        bool IsValid(T value);
    }
}

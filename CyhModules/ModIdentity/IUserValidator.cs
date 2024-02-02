using Cyh.Modules.ModAuthentication;

namespace Cyh.Modules.ModIdentity
{
    /// <summary>
    /// 使用者身分驗證器，通常透過DB存取類別庫繼承
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        /// 使用提供的模型驗證是否是合法使用者，如果使用者不合法，回傳 null
        /// </summary>
        /// <param name="_validateModel">驗證用的模型</param>
        /// <returns>符合條件的使用者，如果找不到，回傳 null</returns>
        IUser? FindUserByLoginModel(ILoginModel? _validateModel);
    }
}

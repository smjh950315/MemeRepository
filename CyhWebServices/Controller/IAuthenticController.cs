using Cyh.Modules.ModAuthentication;
using Cyh.Modules.ModIdentity;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Cyh.WebServices.Controller
{
    /// <summary>
    /// 登入授權用的控制器介面
    /// </summary>
    public interface IAuthenticController : IController
    {
        /// <summary>
        /// 登入選項
        /// </summary>
        IAuthorizationOptions? LoginOptions { get; }

        /// <summary>
        /// 使用者授權驗證器
        /// </summary>
        IUserValidator? UserValidator { get; set; }

        /// <summary>
        /// 使用者帳號登入器
        /// </summary>
        ISignInHelper? SignInHelper { get; set; }

        /// <summary>
        /// 是否在收到登入申請時，無論如何都重新登入
        /// </summary>
        bool AlwaysRelogin { get; set; }

        /// <summary>
        /// 將當前使用者登出
        /// </summary>
        [NonAction]
        void LogUserout();
    }
}

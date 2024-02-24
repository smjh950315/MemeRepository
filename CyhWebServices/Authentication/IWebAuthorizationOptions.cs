using Cyh.Modules.ModAuthentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Cyh.WebServices.Authentication
{
    public interface IWebAuthorizationOptions : IAuthorizationOptions
    {
        /// <summary>
        /// ????
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 是否僅允許HTTP協定
        /// </summary>
        bool HttpOnly { get; }

        /// <summary>
        /// 不知道幹嘛的，預設SameSiteMode.Lax，想了解的請看<see cref="SameSiteMode"/>
        /// </summary>
        SameSiteMode SameSite { get; }

        /// <summary>
        /// Cookie的安全性策略，預設CookieSecurePolicy.SameAsRequest，想了解的請看<see cref="CookieSecurePolicy"/>
        /// </summary>
        CookieSecurePolicy SecurePolicy { get; }

        /// <summary>
        /// 此Cookie對於此APP的功能是否為必須(有用到的話要設定為TRUE)
        /// </summary>
        bool IsEssential { get; }

        /// <summary>
        /// 不知道幹嘛的，預設 <see cref="ChunkingCookieManager"/> 
        /// </summary>
        ChunkingCookieManager CookieManager { get; }

        /// <summary>
        /// 與授權服務有關的路徑
        /// </summary>
        AuthorizationServiceRoutes Routes { get; }

        /// <summary>
        /// 認證用的 Model
        /// </summary>
        ILoginModel? AuthorizationDataModel { get; }
    }
}

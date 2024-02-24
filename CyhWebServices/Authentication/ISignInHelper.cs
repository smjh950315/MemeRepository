using Cyh.Modules.ModAuthentication;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Cyh.WebServices.Authentication
{
    /// <summary>
    /// 登入器的介面
    /// </summary>
    public interface ISignInHelper
    {
        /// <summary>
        /// 加入預先設定好的登入選項
        /// </summary>
        public void SetSignInOptions(IAuthorizationOptions options);

        /// <summary>
        /// 以到目前為止加入的資訊取得Principal
        /// </summary>
        public ClaimsPrincipal Principal { get; }

        /// <summary>
        /// 以到目前為止加入的資訊取得Property(就是登入選項)
        /// </summary>
        public AuthenticationProperties Property { get; }

        /// <summary>
        /// 設定用來產生登入資訊的用戶端ID
        /// </summary>
        /// <param name="clientId">用戶端ID</param>
        public void SetClientId(string clientId);

        /// <summary>
        /// 加入額外的登入資訊，需要加入的資訊視實作方法而定
        /// </summary>
        /// <param name="objects">額外的登入資訊</param>
        public void AddAdditionalInfos(params object?[] objects);
    }
}

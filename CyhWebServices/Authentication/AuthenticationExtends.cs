using Cyh.WebServices.Authentication;
using Cyh.WebServices.Controller;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

namespace Cyh.WebServices.Authentication
{
    public static partial class AuthenticationExtends
    {
        internal static IEnumerable<string> _FetchRoles(string? raw_client_role_string) {
            if (raw_client_role_string.IsNullOrEmpty()) { return Enumerable.Empty<string>(); }
            return raw_client_role_string.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        /// <summary>
        /// 用提供的訊息登入
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerInstance"></param>
        public static void SignInAsync(this ISignInHelper helper, IControllerBase controllerInstance) {
            if (helper == null || controllerInstance == null)
                return;

            controllerInstance.HttpContext
                .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, helper.Principal, helper.Property);
        }

        /// <summary>
        /// 登出當前的使用者
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerInstance"></param>
        public static void SignOutAsync(this ISignInHelper helper, IControllerBase controllerInstance) {
            if (controllerInstance == null)
                return;

            controllerInstance.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 如果已經登入且當前用戶端ID不等於輸入的ID，就登出已登入的用戶端
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerInstance"></param>
        /// <param name="newId">新登入的ID</param>
        public static void SignOutIfIsDifferentId(this ISignInHelper helper, IControllerBase controller, string newId) {
            if (helper == null || controller == null)
                return;

            if (!controller.IsAuthenticated()) { return; }

            string? oldId = controller.GetClientId();

            if (oldId != newId)
                helper.SignOutAsync(controller);
        }

        /// <summary>
        /// 檢查環境並準備登入
        /// </summary>
        /// <param name="newId">準備登入用的ID</param>
        /// <param name="reSigninAnyway">無論如何都重新登入</param>
        /// <returns>是否進行接下來的登入步驟</returns>
        public static bool PrepareSignInAndReady(this ISignInHelper helper, IControllerBase controller, string? newId, bool reSigninAnyway = false) {
            // 缺少必要資訊，無法進行登入
            if (helper == null || controller == null || newId.IsNullOrEmpty())
                return false;

            // 尚未有有登入紀錄，可以進行登入
            if (!controller.IsAuthenticated()) return true;

            string? oldId = controller.GetClientId();
            // 新舊ID不同或是選擇無論如何都重新登入
            if (oldId != newId || reSigninAnyway) {
                helper.SignOutAsync(controller);
                return true;
            }
            //新舊ID相同且不選擇必須重登選項，無須再進行登入
            return false;
        }

        /// <summary>
        /// 不知道幹嘛的，大概是資料安全相關的格式
        /// </summary>
        /// <param name="dataProtectProvider">An interface that can be used to create <see cref="IDataProtector"/> instances</param>
        /// <returns>資料安全相關的格式設定</returns>
        public static ISecureDataFormat<AuthenticationTicket> TicketDataFormat(this IWebAuthorizationOptions options, IDataProtectionProvider dataProtectProvider) {
            IDataProtector dataProtecter = dataProtectProvider.CreateProtector(CookieAuthenticationDefaults.AuthenticationScheme);
            return new TicketDataFormat(dataProtecter);
        }

        /// <summary>
        /// 取得當前客戶端的識別ID
        /// </summary>
        /// 當前客戶端的識別ID，如果未設定，回傳null
        public static string? GetClientId(this ClaimsPrincipal? principal) {
            return principal?.Identity?.Name;
        }

        /// <summary>
        /// 取得當前客戶端的權限名稱集合
        /// </summary>
        /// <returns>當前客戶端的權限名稱集合</returns>
        public static IEnumerable<string> GetClientRoles(this ClaimsPrincipal? principal) {
            if (principal == null) { return Enumerable.Empty<string>(); }
            string? rawString = principal.Claims.FirstOrDefault(x => x.ValueType == ClaimDefinitions.ClientRole)?.Value;
            return _FetchRoles(rawString);
        }
    }
}

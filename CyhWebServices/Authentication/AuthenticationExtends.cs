using Cyh.WebServices.Controller;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Cyh.WebServices.Authentication
{
    public static partial class AuthenticationExtends
    {
        /// <summary>
        /// 用提供的訊息登入
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="controllerInstance"></param>
        public static void SignInAsync(this ISignInHelper helper, ControllerBase controllerInstance) {
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
        public static void SignOutAsync(this ISignInHelper helper, ControllerBase controllerInstance) {
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
        public static void SignOutIfIsNotId(this ISignInHelper helper, MyControllerBase controllerInstance, string newId) {
            if (helper == null || controllerInstance == null)
                return;

            if (!controllerInstance.IsLogin)
                return;

            string? oldId = controllerInstance.CurrentUser?.Id;
            if (oldId != newId)
                helper.SignOutAsync(controllerInstance);
        }

        /// <summary>
        /// 檢查環境並準備登入
        /// </summary>
        /// <param name="newId">準備登入用的ID</param>
        /// <param name="reSigninAnyway">無論如何都重新登入</param>
        /// <returns>是否進行接下來的登入步驟</returns>
        public static bool PrepareSignInAndReady(this ISignInHelper helper, MyControllerBase controllerInstance, string? newId, bool? reSigninAnyway = null) {
            // 缺少必要資訊，無法進行登入
            if (helper == null || controllerInstance == null || newId.IsNullOrEmpty())
                return false;

            // 尚未有有登入紀錄，可以進行登入
            if (!controllerInstance.IsLogin)
                return true;

            string? oldId = controllerInstance.CurrentUser?.Id;
            // 新舊ID不同或是選擇無論如何都重新登入
            if (oldId != newId || reSigninAnyway.NullOr(false)) {
                helper.SignOutAsync(controllerInstance);
                return true;
            }
            //新舊ID相同且不選擇必須重登選項，無須再進行登入
            return false;
        }
    }
}

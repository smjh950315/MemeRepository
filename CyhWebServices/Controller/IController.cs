using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Cyh.WebServices.Controller
{
    /// <summary>
    /// 基本 Controller 的介面
    /// </summary>
    public partial interface IController : IControllerBase
    {
        public IWebAppConfigurations? _AppConfigurations { get; set; }

        /// <summary>
        /// 當前的使用者資料
        /// </summary>
        public PrincipalReader? CurrentUser => User;

        /// <summary>
        /// 是否為登入狀態
        /// </summary>
        public bool IsLogin => CurrentUser?.IsAuthenticated ?? false;

        /// <summary>
        /// 首頁
        /// </summary>
        public IActionResult HomePage => RedirectToAction("Index", "Home");

        /// <summary>
        /// 登入頁面
        /// </summary>
        public virtual IActionResult AccountPage => RedirectToAction("Index", "Account");
    }
}

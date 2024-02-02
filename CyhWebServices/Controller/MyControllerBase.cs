using Cyh.DataModels;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cyh.WebServices.Controller
{
    public abstract class MyControllerBase : Microsoft.AspNetCore.Mvc.Controller, IController
    {
        public IWebAppConfigurations? _AppConfigurations { get; set; }
        public IController BaseController => this;
        public PrincipalReader? CurrentUser => this.User;
        public virtual bool IsLogin => this.CurrentUser?.IsAuthenticated ?? false;
        public IDataTransResult EmptyResult => new DataTransResultBase()
        {
            BeginTime = DateTime.Now,
            Accesser = this.CurrentUser?.Id ?? "UNKNOW",
        };
        public IDataTransResult NoLoginResult => new DataTransResultBase()
        {
            BeginTime = DateTime.Now,
            Accesser = this.CurrentUser?.Id ?? "UNKNOW",
            Message = "尚未登入"
        };

        /// <summary>
        /// 嘗試將輸入可能為JSON的物件轉成某個MODEL
        /// </summary>
        /// <typeparam name="TModel">要轉的MODEL型別</typeparam>
        /// <param name="json">可能為JSON的物件</param>
        /// <returns>取得的MODEL，如果失敗回傳NULL</returns>
        public static TModel? DeserializeFromJson<TModel>(object? json) where TModel : class {
            string? str = json?.ToString();

            if (str == null)
                return null;

            return TryGetValue(fn => JsonSerializer.Deserialize<TModel>(str), null);
        }

        /// <summary>
        /// 嘗試將MODEL字串化
        /// </summary>
        /// <typeparam name="TModel">來源的MODEL型別</typeparam>
        /// <param name="model">來源的MODEL</param>
        /// <returns>字串化的JSON物件</returns>
        public static string SerializeToJson<TModel>(TModel? model) where TModel : class {
            if (model == null)
                return string.Empty;

            return TryGetValue(fn => JsonSerializer.Serialize(model), null) ?? "";
        }

        /// <summary>
        /// 首頁
        /// </summary>
        public IActionResult HomePage => this.RedirectToAction("Index", "Home");

        /// <summary>
        /// 登入頁面，預設為 Account/Index，如要更改預設值，使用 override 覆寫
        /// </summary>
        public virtual IActionResult AccountPage => this.RedirectToAction("Index", "Account");

        public MyControllerBase(IWebAppConfigurations webAppConfigurations) {
            this._AppConfigurations = webAppConfigurations;
        }
    }
}

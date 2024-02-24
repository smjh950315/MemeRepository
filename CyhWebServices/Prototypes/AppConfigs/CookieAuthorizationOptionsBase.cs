using Cyh.Modules.ModAuthentication;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Cyh.WebServices.Prototypes.AppConfigs
{
    public class CookieAuthorizationOptionsBase : IWebAuthorizationOptions
    {
        private string? _Name;
        private string? _Path;
        private ChunkingCookieManager? _CookieManager;
        private AuthorizationServiceRoutes? _Routes;

        /// <summary>
        /// 要使用的Cookie名稱(通常與APP名稱相同)
        /// </summary>
        public string Name {
            get => this._Name ?? "unamed";
            set => this._Name = value;
        }

        /// <summary>
        /// ????
        /// </summary>
        public string Path {
            get => this._Path ?? "unsetted";
            set => this._Path = value;
        }

        /// <summary>
        /// 是否僅允許HTTP協定
        /// </summary>
        public bool HttpOnly { get; set; }

        public TimeSpan LifeTime { get; set; }

        public bool AllowRefresh { get; set; }

        public bool IsPersistent { get; set; }

        /// <summary>
        /// 是否在每次呼叫時重置Cookie生命週期
        /// </summary>
        public bool SlidingLiftTime { get; set; }

        /// <summary>
        /// 不知道幹嘛的，預設SameSiteMode.Lax，想了解的請看<see cref="SameSiteMode"/>
        /// </summary>
        public SameSiteMode SameSite { get; set; }

        /// <summary>
        /// Cookie的安全性策略，預設CookieSecurePolicy.SameAsRequest，想了解的請看<see cref="CookieSecurePolicy"/>
        /// </summary>
        public CookieSecurePolicy SecurePolicy { get; set; }

        /// <summary>
        /// 此Cookie對於此APP的功能是否為必須(有用到的話要設定為TRUE)
        /// </summary>
        public bool IsEssential { get; set; }

        /// <summary>
        /// 不知道幹嘛的，預設 <see cref="ChunkingCookieManager"/> 
        /// </summary>
        public ChunkingCookieManager CookieManager {
            get => this._CookieManager ?? new();
            set => this._CookieManager = value;
        }

        public AuthorizationServiceRoutes Routes {
            get {
                if (this._Routes == null) {
                    this._Routes = new AuthorizationServiceRoutes();
                }
                return this._Routes;
            }
            set { this._Routes = value; }
        }

        public ILoginModel? AuthorizationDataModel { get; set; }
    }
}

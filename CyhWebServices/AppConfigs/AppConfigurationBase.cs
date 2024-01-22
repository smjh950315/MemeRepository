using Cyh.Modules.ModAuthentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyh.WebServices.AppConfigs
{
    public abstract class AppConfigurationBase : IWebAppConfigurations
    {
        public abstract string ApplicationName { get; }

        public virtual ICookieAuthenticOptions CookieAuthenticOptions => new DefaultCookieAuthenticOptions() {
            Name = ApplicationName,
            Path = "/",
            HttpOnly = false,
            CookieAge = 300,
            SlidingExpiration = true,
            SameSite = SameSiteMode.None,
            SecurePolicy = CookieSecurePolicy.SameAsRequest,
            IsEssential = true,
            CookieManager = new Microsoft.AspNetCore.Authentication.Cookies.ChunkingCookieManager()
        };

        public virtual ILoginOptions LoginOptions => throw new NotImplementedException();

        public virtual IRouteSettings RouteSettings => new DefaultAppRoutes() {
            Login = "Account/Login",
            Logout = "Account/Logout",
            AccessDenied = "/Account/AccessDenied",
            ReturnUrlParameter = "returnUrl",
            Error = "/Home/Error"
        };

        public virtual IViewSettings ViewSettings => new DefaultViewSettings() {
            Title_CurrentUser = "目前使用者: ",
            Title_Default = "Untitled",
            Display_LoginState = true,
            Display_AppTitle = true,
            Display_ActionName = false
        };
    }
}

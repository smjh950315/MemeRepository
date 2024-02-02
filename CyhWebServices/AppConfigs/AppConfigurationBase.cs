using Cyh.Modules.ModAuthentication;
using Microsoft.AspNetCore.Http;

namespace Cyh.WebServices.AppConfigs
{
    public abstract class AppConfigurationBase : IWebAppConfigurations
    {
        public abstract string ApplicationName { get; }

        public virtual ICookieAuthenticOptions CookieAuthenticOptions => new DefaultCookieAuthenticOptions()
        {
            Name = this.ApplicationName,
            Path = "/",
            HttpOnly = false,
            CookieAge = 300,
            SlidingExpiration = true,
            SameSite = SameSiteMode.None,
            SecurePolicy = CookieSecurePolicy.SameAsRequest,
            IsEssential = true,
            CookieManager = new Microsoft.AspNetCore.Authentication.Cookies.ChunkingCookieManager()
        };

        public abstract IMyAuthorizationOptions LoginOptions { get; }

        public virtual IRouteSettings RouteSettings => new DefaultAppRoutes()
        {
            Login = "/Account/Login",
            Logout = "/Account/Logout",
            AccessDenied = "/Account/AccessDenied",
            ReturnUrlParameter = "returnUrl",
            Error = "/Home/Error"
        };

        public virtual IViewSettings ViewSettings => new DefaultViewSettings()
        {
            Title_CurrentUser = "目前使用者: ",
            Title_Default = "Untitled",
            Display_LoginState = true,
            Display_AppTitle = true,
            Display_ActionName = false
        };
    }
}

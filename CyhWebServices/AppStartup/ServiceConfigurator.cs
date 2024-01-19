using Cyh.WebServices.AppConfigs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace Cyh.WebServices.AppStartup
{
    public class ServiceConfigurator
    {
        public static void Configuration(ref IServiceCollection _Services, IWebAppConfigurations _AppConf)
        {
            IRouteSettings _Route = _AppConf.RouteSettings;
            ICookieAuthenticOptions _Cookie = _AppConf.CookieAuthenticOptions;
            _Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
              .Configure<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider>((options, dp) =>
              {
                  options.LoginPath = _Route.Login;
                  options.LogoutPath = _Route.Logout;
                  options.AccessDeniedPath = _Route.AccessDenied;
                  options.ReturnUrlParameter = _Route.ReturnUrlParameter;
                  options.Cookie.MaxAge = _Cookie.MaxCookieAge;
                  options.SlidingExpiration = _Cookie.SlidingExpiration;
                  options.Cookie.Name = _Cookie.Name;
                  options.Cookie.Path = _Cookie.Path;
                  options.Cookie.SameSite = _Cookie.SameSite;
                  options.Cookie.HttpOnly = _Cookie.HttpOnly;
                  options.Cookie.SecurePolicy = _Cookie.SecurePolicy;
                  options.Cookie.IsEssential = _Cookie.IsEssential;
                  options.CookieManager = _Cookie.CookieManager;

                  options.DataProtectionProvider ??= dp;
                  options.TicketDataFormat = _Cookie.TicketDataFormat(dp);

                  options.Events.OnSigningIn = context => Task.CompletedTask;
                  options.Events.OnSignedIn = context => Task.CompletedTask;
                  options.Events.OnSigningOut = context => Task.CompletedTask;
                  options.Events.OnValidatePrincipal += context => Task.CompletedTask;
              });
            _Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}

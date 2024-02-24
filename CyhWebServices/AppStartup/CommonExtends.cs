using Cyh.MyException;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cyh.WebServices.AppStartup
{
    public static partial class CommonExtends
    {
        /// <summary>
        /// Inject human-friendly descriptions for Operations, Parameters and Schemas based on XML Comment files
        /// </summary>
        /// <param name="swaggerGenOptions"></param>
        /// <param name="xmlDocFactory">A factory method that returns XML Comments as an XPathDocument</param>
        /// <param name="includeControllerXmlComments">
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </param>
        public static void MyIncludeXmlComments(
            this SwaggerGenOptions swaggerGenOptions,
            IEnumerable<string> xmlPaths,
            bool includeControllerXmlComments = false) {
            if (xmlPaths.IsNullOrEmpty())
                return;

            foreach (string xmlPath in xmlPaths) {
                if (File.Exists(xmlPath))
                    swaggerGenOptions.IncludeXmlComments(xmlPath, includeControllerXmlComments);
            }
        }

        /// <summary>
        /// 註冊 Controller 服務
        /// </summary>
        public static void RegisterControllerService(this MyStartup startup, Action<MvcOptions>? configure = null, Action<JsonOptions>? jsonOption = null) {
            if (MyStartup.UseController)
                return;

            if (configure != null) {
                if (jsonOption != null) {
                    startup.Services?.AddControllers(configure).AddJsonOptions(jsonOption);
                } else {
                    startup.Services?.AddControllers(configure);
                }
            } else {
                if (jsonOption != null) {
                    startup.Services?.AddControllers().AddJsonOptions(jsonOption);
                } else {
                    startup.Services?.AddControllers();
                }
            }
            MyStartup.UseController = true;
        }

        /// <summary>
        /// 註冊跨域請求服務
        /// </summary>
        /// <param name="configure">跨域請求的策略</param>
        public static void RegisterDefaultCORSPolicy(this MyStartup startup,
            Action<CorsPolicyBuilder> configure) {
            if (MyStartup.UseCors)
                return;

            if (configure != null)
                startup.Services?.AddCors(opts => {
                    opts.AddDefaultPolicy(configure);
                });
            else
                startup.Services?.AddCors();

            MyStartup.UseCors = true;
        }

        /// <summary>
        /// 註冊 Cookie 選項
        /// </summary>
        public static void RegisterCookieOptions(this MyStartup startup, IWebAuthorizationOptions cookie) {
            if (MyStartup.UseCookie)
                return;

            AuthorizationServiceRoutes route = cookie.Routes;
            startup.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
                .Configure<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider>((options, dataProvider) => {
                    options.LoginPath = route.Login;
                    options.LogoutPath = route.Logout;
                    options.AccessDeniedPath = route.AccessDenied;
                    options.ReturnUrlParameter = startup.AppConfigurations.ReturnUrlParameter;
                    options.SlidingExpiration = cookie.SlidingLiftTime;
                    options.Cookie.Name = cookie.Name;
                    options.Cookie.Path = cookie.Path;
                    options.Cookie.SameSite = cookie.SameSite;
                    options.Cookie.HttpOnly = cookie.HttpOnly;
                    options.Cookie.SecurePolicy = cookie.SecurePolicy;
                    options.Cookie.IsEssential = cookie.IsEssential;
                    options.CookieManager = cookie.CookieManager;

                    options.DataProtectionProvider ??= dataProvider;
                    options.TicketDataFormat = cookie.TicketDataFormat(dataProvider);

                    options.Events.OnSigningIn = context => Task.CompletedTask;
                    options.Events.OnSignedIn = context => Task.CompletedTask;
                    options.Events.OnSigningOut = context => Task.CompletedTask;
                    options.Events.OnValidatePrincipal += context => Task.CompletedTask;
                });
            startup.Services?.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            MyStartup.UseCookie = true;
        }

        /// <summary>
        /// 註冊 IIS 選項
        /// </summary>
        /// <param name="configure">IIS 選項</param>
        public static void RegisterIISOptions(this MyStartup startup,
            Action<IISOptions> configure) {

            if (MyStartup.UseIISOptions)
                return;

            startup.Services?.Configure(configure);

            MyStartup.UseIISOptions = true;
        }

        /// <summary>
        /// 註冊 Razor 頁面服務
        /// </summary>
        /// <param name="configure"></param>
        public static void RegisterRazorPages(this MyStartup startup, Action<RazorPagesOptions>? configure = null) {
            if (MyStartup.UseRazorPages)
                return;

            if (configure != null)
                startup.Services?.AddRazorPages(configure);
            else
                startup.Services?.AddRazorPages();

            MyStartup.UseRazorPages = true;
        }

        /// <summary>
        /// 註冊額外的表單選項
        /// </summary>
        /// <param name="startup"></param>
        /// <param name="configure"></param>
        public static void RegisterFormOptions(this MyStartup startup, Action<FormOptions> configure) {
            if (MyStartup.UseFormOptions)
                return;

            if (configure != null)
                startup.Services?.Configure(configure);

            MyStartup.UseFormOptions = true;
        }

        /// <summary>
        /// 註冊 SignalR 服務
        /// </summary>
        /// <param name="configure"></param>
        public static void RegisterSignalR(this MyStartup startup, Action<HubOptions>? configure = null) {
            if (MyStartup.UseSignalR)
                return;

            if (configure != null)
                startup.Services?.AddSignalR(configure);
            else
                startup.Services?.AddSignalR();

            MyStartup.UseSignalR = true;
        }

        /// <summary>
        /// 註冊 Swagger 功能
        /// </summary>
        /// <param name="startup"></param>
        /// <param name="setupAction"></param>
        /// <exception cref="MyArgumentException"></exception>
        public static void RegisterSwagger(this MyStartup startup, Action<SwaggerGenOptions>? setupAction = null) {
            if (MyStartup.UseSwagger)
                return;

            if (startup.Services == null)
                throw new MyArgumentException(ArgumentDetail.NewDetails<IServiceCollection>("startup.Services"));

            startup.Services?.AddEndpointsApiExplorer();
            if (setupAction != null) {
                startup.Services.AddSwaggerGen(setupAction);
            } else {
                startup.Services.AddSwaggerGen();
            }

            MyStartup.UseSwagger = true;
        }
    }
}

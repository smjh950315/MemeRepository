using Cyh.MyException;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cyh.WebServices.AppStartup
{
    public static partial class MyServiceExtends
    {
        /// <summary>
        /// 註冊 Controller 服務
        /// </summary>
        public static void RegisterControllerService(this MyStartup startup, Action<MvcOptions>? configure = null) {
            if (MyStartup.UseController)
                return;

            if (configure != null)
                startup.Services?.AddControllers(configure);
            else
                startup.Services?.AddControllers();

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
        public static void RegisterCookieOptions(this MyStartup startup) {
            if (MyStartup.UseCookie)
                return;

            var route = startup.AppConfigurations.RouteSettings;
            var cookie = startup.AppConfigurations.CookieAuthenticOptions;
            startup.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
                .Configure<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider>((options, dataProvider) => {
                    options.LoginPath = route.Login;
                    options.LogoutPath = route.Logout;
                    options.AccessDeniedPath = route.AccessDenied;
                    options.ReturnUrlParameter = route.ReturnUrlParameter;
                    options.Cookie.MaxAge = cookie.MaxCookieAge();
                    options.SlidingExpiration = cookie.SlidingExpiration;
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


    /// <summary>
    /// 將App執行所需要基本的設定預先包含，並將額外選項抽離到虛擬函數來設定
    /// </summary>
    public abstract partial class MyStartup
    {
        public IServiceCollection? Services { get; private set; }
        public IConfiguration? Configuration { get; }

        public abstract IWebAppConfigurations AppConfigurations { get; }

        /// <summary>
        /// Startup 類需要的函數，注入用的地方
        /// </summary>
        /// <param name="services"></param>
        public abstract void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 打算在靜態函數 Startup 類中 Configure (IApplicationBuilder app, IWebHostEnvironment env) 執行的函數，使用時必須以 override 複寫
        /// </summary>
        /// <param name="app"></param>
        public virtual void AddAdditionalConfigs(IApplicationBuilder app) { }

        public MyStartup(IConfiguration configuration) {
            this.Configuration = configuration;
            Callback_AddConfigsToAppBuilder = AddAdditionalConfigs;
        }

        public void SetServiceSource<T>(IServiceCollection? services) where T : class, IWebAppConfigurations, new() {

            if (services == null)
                throw new MyArgumentException(ArgumentDetail.NewDetails<IServiceCollection>(nameof(services)));

            this.Services = services;

            this.Services.AddScoped<IWebAppConfigurations, T>();

            Callback_AddConfigsToAppBuilder = AddAdditionalConfigs;

            this.Services.AddExceptionHandler(e => {
                e.ExceptionHandlingPath = "/Error";
                e.AllowStatusCode404Response = true;
            });
        }

        public static void SetStartupRequirements(IApplicationBuilder app, IWebHostEnvironment env) {
            if (app == null || env == null)
                throw new MyArgumentException(
                    ArgumentDetail.NewDetails<IApplicationBuilder>("app", app == null),
                    ArgumentDetail.NewDetails<IWebHostEnvironment>("env", env == null));

            MyStartup.ApplicationBuilder = app;
            MyStartup.WebHostEnvironment = env;
        }

    }
}

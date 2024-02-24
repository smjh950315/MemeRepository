using Cyh.MyException;
using Cyh.WebServices.AppConfigs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cyh.WebServices.AppStartup
{
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
            Callback_AddConfigsToAppBuilder = this.AddAdditionalConfigs;
        }

        public void SetServiceSource<T>(IServiceCollection? services) where T : class, IWebAppConfigurations, new() {

            if (services == null)
                throw new MyArgumentException(ArgumentDetail.NewDetails<IServiceCollection>(nameof(services)));

            this.Services = services;

            this.Services.AddScoped<IWebAppConfigurations, T>();

            Callback_AddConfigsToAppBuilder = this.AddAdditionalConfigs;

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

using Cyh.MyException;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Cyh.WebServices.AppStartup
{
    public abstract partial class MyStartup
    {
        delegate void FnAddConfigsToAppBuilder(IApplicationBuilder appBuilder);

#pragma warning disable CS8618
        public static IApplicationBuilder ApplicationBuilder { get; set; }
        public static IWebHostEnvironment WebHostEnvironment { get; set; }
        private static FnAddConfigsToAppBuilder Callback_AddConfigsToAppBuilder { get; set; }
#pragma warning restore CS8618

        internal static bool UseController = false;
        internal static bool UseCors = false;
        internal static bool UseIISOptions = false;
        internal static bool UseCookie = false;

        internal static bool UseSwagger = false;
        internal static bool UseRazorPages = false;
        internal static bool UseFormOptions = false;
        internal static bool UseSignalR = false;

        internal static bool HasRegisteredCors = false;
        internal static bool HasRegisteredCookie = false;
        internal static bool HasRegisteredSignalR = false;
        internal static bool HasRegisteredAuthentication = false;
        internal static bool HasRegisteredStaticFiles = false;
        internal static bool HasRegisteredRouting = false;
        internal static bool HasRegisteredEndPoints = false;
        internal static bool HasRegisteredSwagger = false;

        public static void AutoRegisterDependServices() {

            if (WebHostEnvironment == null)
                throw new MyArgumentException(ArgumentDetail.NewDetails<IWebHostEnvironment>("env", WebHostEnvironment == null));

            if (ApplicationBuilder == null)
                throw new MyArgumentException(ArgumentDetail.NewDetails<IWebHostEnvironment>("app", ApplicationBuilder == null));

            if (WebHostEnvironment.IsDevelopment())
                ApplicationBuilder.UseDeveloperExceptionPage();
            else
                ApplicationBuilder.UseExceptionHandler();


            if (UseCors)
                RegisterCORS();

            if (UseSwagger)
                RegisterSwaggerUI();

            if (Callback_AddConfigsToAppBuilder != null)
                Callback_AddConfigsToAppBuilder(ApplicationBuilder);

            if (UseCookie)
                RegisterAuthentication();
        }

    }
}

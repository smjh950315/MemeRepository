using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Swagger;

namespace Cyh.WebServices.AppStartup
{
    public abstract partial class MyStartup
    {
        public static void RegisterCORS() {
            if (HasRegisteredCors) return;
            ApplicationBuilder.UseCors();
            HasRegisteredCors = true;
        }

        public static void RegisterAuthentication() {
            if (HasRegisteredAuthentication) return;
            ApplicationBuilder.UseAuthentication();
            HasRegisteredAuthentication = true;
        }

        public static void RegisterStaticFiles() {
            if (HasRegisteredStaticFiles) return;
            ApplicationBuilder.UseStaticFiles();
            HasRegisteredStaticFiles = true;
        }

        public static void RegisterRouting() {
            if (HasRegisteredRouting) return;
            ApplicationBuilder.UseRouting();
            HasRegisteredRouting = true;
        }

        public static void RegisterSwaggerUI(Action<SwaggerOptions>? setupAction = null) {
            if (HasRegisteredSwagger)
                return;

            if (setupAction != null)
                ApplicationBuilder.UseSwagger(setupAction);
            else
                ApplicationBuilder.UseSwagger();

            ApplicationBuilder.UseSwaggerUI();

            HasRegisteredSwagger = true;
        }

        public static void RegisterEndPoints(Action<IEndpointRouteBuilder>? configure = null) {
            if (HasRegisteredEndPoints)
                return;

            if (configure == null) {
                if (!HasRegisteredRouting) {
                    RegisterRouting();
                }
                ApplicationBuilder.UseEndpoints(endpoints => {
                    endpoints.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}"
                        );
                    endpoints.MapRazorPages();
                });
            } else {
                if (!HasRegisteredRouting) {
                    RegisterRouting();
                }
                ApplicationBuilder.UseEndpoints(configure);
            }

            HasRegisteredEndPoints = true;
        }
    }
}

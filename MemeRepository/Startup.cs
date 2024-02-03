using Cyh.DataHelper;
using Cyh.EFCore.Interface;
using Cyh.WebServices.AppConfigs;
using Cyh.WebServices.AppStartup;
using MemeRepository.Db.Accesser;
using MemeRepository.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

// Scaffold-DbContext "Server=localhost,1433; user id=sa; password=0000; Database=MemeRepository;integrated security=false;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -NoPluralize -UseDatabaseNames -Context MemeRepositoryContext -OutputDir Models -Force

namespace MemeRepository
{
    public class Startup : Cyh.WebServices.AppStartup.MyStartup
    {
        public override IWebAppConfigurations AppConfigurations { get; }

#pragma warning disable CS8618
        public Startup(IConfiguration configuration) : base(configuration) {

        }
#pragma warning restore CS8618

        public override void ConfigureServices(IServiceCollection services) {
            this.SetServiceSource<AppConfigs.AppConfigurations>(services);

            // 可用的部分:
            // UseControllers
            // IISOptions
            // CORS
            // RazorPages
            // SignalR
            // FormOptions
            // Authentic(Cookie)

            this.RegisterControllerService();

            this.RegisterDefaultCORSPolicy(c => c.AllowAnyOrigin());

            // this.RegisterIISOptions(iis => {
            //     iis.ForwardClientCertificate = false;
            //     iis.AutomaticAuthentication = false;
            // });

            this.RegisterSwagger(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            // 如果註解這段，程式可以正常執行但是需要用到DB的功能會失效
            services.AddDbContext<MemeRepositoryContext>(options => {
                options.UseSqlServer(this.Configuration.GetConnectionString("MainDB"));
            });

            services.AddScoped<IDbContext, MyDbContext>();
            services.AddScoped<IDataManagerActivator, MyDataActivator>();
            services.AddScoped<IDataManager<IMAGE>, DataManagerBase<IMAGE>>();

            this.RegisterRazorPages();

            this.RegisterSignalR();

        }
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            MyStartup.SetStartupRequirements(app, env);

            // 可用的部分:
            // StaticFiles
            // Routing (RegisterEndPoints 之前會先檢查並自應執行)
            // Authentication
            // Endpoints

            RegisterStaticFiles();

            AutoRegisterDependServices();

            RegisterEndPoints();
        }

    }
}

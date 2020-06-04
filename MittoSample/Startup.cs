using Funq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MittoSample.Helper;
using MittoSample.Repository;
using MittoSample.ServiceInterface;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MittoSample
{
    public class Startup : ModularStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("MittoSample", typeof(SMSServices).Assembly) { }

        // Configure AppHost with the necessary configuration and dependencies
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
            });

            //Set database provider and connection string
            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(AppSettings.GetString("Database:MySqlDbConnection"), MySqlDialect.Provider));

            //Registring dependencies from repository layer
            container.RegisterAutoWiredAs<SMSRepository, ISMSRepository>().ReusedWithin(ReuseScope.None);
            container.RegisterAutoWiredAs<CountryRepository, ICountryRepository>().ReusedWithin(ReuseScope.None);

            //Create database schema and insert default data
            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                Database.Create(db);
            }
        }
    }
}

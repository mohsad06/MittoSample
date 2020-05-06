using Funq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MittoSample.Logic;
using MittoSample.Logic.Repository;
using MittoSample.ServiceInterface;
using MittoSample.ServiceModel.Types;
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

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
            });

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(AppSettings.GetString("Database:MySqlDbConnection"), MySqlDialect.Provider));

            container.RegisterAutoWiredAs<SMSRepository, ISMSRepository>().ReusedWithin(ReuseScope.None);
            container.RegisterAutoWiredAs<CountryRepository, ICountryRepository>().ReusedWithin(ReuseScope.None);

            container.RegisterAutoWiredAs<SMSLogic, ISMSLogic>().ReusedWithin(ReuseScope.None);
            container.RegisterAutoWiredAs<CountryLogic, ICountryLogic>().ReusedWithin(ReuseScope.None);

            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                if (!db.TableExists<SMS>())
                    db.CreateTable<SMS>();

                if (!db.TableExists<Country>())
                {
                    db.CreateTable<Country>();

                    #region AddDefaultCountries

                    db.Insert(new Country { Name = "Germany", CountryCode = "49", MobileCountryCode = "262", PricePerSMS = 0.055m });
                    db.Insert(new Country { Name = "Austria", CountryCode = "43", MobileCountryCode = "232", PricePerSMS = 0.053m });
                    db.Insert(new Country { Name = "Poland", CountryCode = "48", MobileCountryCode = "260", PricePerSMS = 0.032m });

                    #endregion
                }
            }
        }
    }
}

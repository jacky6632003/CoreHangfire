using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CoreHangfire.Infrastructure.Dependency;
using CoreHangfire.Infrastructure.HangFireMisc;
using CoreHangfire.Infrastructure.HangFireMisc.Interface;
using CoreHangfire.Infrastructure.Mapping;
using Hangfire;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;

namespace CoreHangfire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string SampleConnection { get; set; }

        private string HangfireConnection { get; set; }

        private HangfireSettings HangfireSettings { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // AutoMapper Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ControllerMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            // Implement http client with polly policy.
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                                                  .WaitAndRetryAsync(new[]
                                                  {
                                                      TimeSpan.FromSeconds(1),
                                                      TimeSpan.FromSeconds(5),
                                                      TimeSpan.FromSeconds(10)
                                                  });

            services.AddHttpClient("HT")
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        Proxy = new WebProxy(""),
#if DEBUG
                        UseProxy = true,
                        UseDefaultCredentials = true
#endif
                    })
                    .AddPolicyHandler(retryPolicy);

            services.AddControllersWithViews()
                    .AddControllersAsServices();

            // database connection - Sample
            this.SampleConnection = this.Configuration.GetConnectionString("Sample");

            // database connection - Hangfire
            this.HangfireConnection = this.Configuration.GetConnectionString("Hangfire");

            // Dendency Injection
            services.AddDendencyInjection();

            // HangfireSettings
            var hangfireSettings = new HangfireSettings();
            this.Configuration.GetSection("HangfireSettings").Bind(hangfireSettings);
            this.HangfireSettings = hangfireSettings;

            // Hangfire
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage
                (
                    nameOrConnectionString: this.HangfireConnection,
                    options: new SqlServerStorageOptions
                    {
                        SchemaName = hangfireSettings.SchemaName,
                        PrepareSchemaIfNecessary = hangfireSettings.PrepareSchemaIfNecessary,
                        JobExpirationCheckInterval = TimeSpan.FromSeconds(60),
                    }
                );

                config.UseConsole();
                config.UseNLogLogProvider();

                config.UseDashboardMetric(SqlServerStorage.ActiveConnections);
                config.UseDashboardMetric(SqlServerStorage.TotalConnections);
                config.UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount);
                config.UseDashboardMetric(DashboardMetrics.ProcessingCount);
                config.UseDashboardMetric(DashboardMetrics.FailedCount);
                config.UseDashboardMetric(DashboardMetrics.SucceededCount);
            });

            if (this.HangfireSettings.EnableServer.Equals(true))
            {
                services.AddHangfireServer();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //---------------------------------------------
            // Hangfire

            if (this.HangfireSettings.EnableDashboard)
            {
                app.UseHangfireDashboard
                (
                    "/hangfire",
                    new DashboardOptions
                    {
                        Authorization = new[] { new HangfireAuthorizeFilter() },
                        IgnoreAntiforgeryToken = true
                    }
                );
            }

            if (this.HangfireSettings.EnableServer.Equals(false))
            {
                return;
            }

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                ServerName = $"{Environment.MachineName}:{this.HangfireSettings.ServerName}",
                WorkerCount = this.HangfireSettings.WorkerCount,
                Queues = new[] { "default" }
            });

            var hangfireJobTrigger = serviceProvider.GetService<IHangfireJobTrigger>();
            hangfireJobTrigger.OnStart();
        }
    }
}
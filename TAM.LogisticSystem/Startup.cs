using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Services;
using Hangfire;
using StackExchange.Redis;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem
{
    public class Startup
    {
        public static ConnectionMultiplexer Redis { get; private set; }

        private readonly bool DebugMode;
        private readonly bool ForceHttps;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            this.DebugMode = env.IsDevelopment() || env.EnvironmentName == "AccelistAzureDev";
            this.ForceHttps = env.IsProduction();
            Redis = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.RollingFile(@"C:\AspNetCoreLog\TLS-{Date}.log", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Accelist(new Guid("12147f27-891d-4390-9903-d89963ade0cb"))
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options =>
            {
                options.OutputFormatters.RemoveType<StringOutputFormatter>();

                if (ForceHttps)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                options.Filters.Add(new ResponseCacheAttribute
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
            });

            AddEnvironmentServices(services);
            AddStorageServices(services);
            AddLogisticSystemServices(services);
        }

        private void AddEnvironmentServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddConfiguration<ConfigurationWatch>(Configuration.GetSection("Watch"));
            services.AddScoped<WebEnvironmentService>();
        }

        private void AddLogisticSystemServices(IServiceCollection services)
        {
            services.AddTransient(ioc =>
            {
                var env = ioc.GetService<WebEnvironmentService>();
                return new Passport.SDK.PassportApi(env.TamPassportUrl, env.TamPassportAppId);
            });

            services.AddTransient<AfiDownloadService>();
            services.AddTransient<AfiHOApprovalService>();
            services.AddTransient<AfiReceiveDocumentService>();
            services.AddTransient<AfiRequestRevisiAndExCancelFormService>();
            services.AddTransient<AfiRequestRevisiAndExCancelService>();
            services.AddTransient<AfiRequestService>();
            //services.AddTransient<AfiRequestUploadService>();
            services.AddTransient<AfiReturnToOutletService>();
            services.AddTransient<AfiReturnToOutletFormService>();
            services.AddTransient<UnitAssignService>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<BranchService>();
            services.AddTransient<BrandService>();
            services.AddTransient<CancelDeliveryRequestService>();
            services.AddTransient<CarTypeService>();
            services.AddTransient<PDCDeliveryMethodService>();
            // TIE: START
            // services.AddTransient<CBUFinalizePIBService>();
            // TIE: END
            services.AddTransient<CityLegService>();
            services.AddTransient<CityMasterService>();
            services.AddTransient<ClusterService>();
            services.AddTransient<ColourService>();
            services.AddTransient<CompanyMasterService>();
            services.AddTransient<ConfigurationPlanningService>();
            services.AddTransient<CreateLogisticPlanService>();

            // TIE: START
            // services.AddTransient<DccpReadinessVolumeService>();
            // TIE: END
            services.AddTransient<DealerMasterService>();
            // TIE: START
            // services.AddTransient<DefectMaintenanceService>();
            // TIE: END
            services.AddTransient<DeliveryLegLeadTimeService>();
            services.AddTransient<DeliveryLegService>();
            services.AddTransient<DeliveryRequestService>();
            services.AddTransient<DeliveryUnitAdvanceService>();
            services.AddTransient<DeliveryUnitLoadingService>();
            services.AddTransient<DMSService>();
            services.AddTransient<DownloadDccpReadinessVolumeService>();
            services.AddTransient<DwellingTimeService>();
            services.AddTransient<EngineService>();
            services.AddTransient<ExcelUploadService>();
            // TIE: START
            // services.AddTransient<ExchangeRateService>();
            // TIE: END
            services.AddTransient<FormARequestService>();
            services.AddTransient<FormAService>();
            services.AddTransient<GenerateJamBreakService>();
            services.AddTransient<GeneratePolaRangkaianRuteService>();
            services.AddTransient<GenerateShiftKerjaService>();
            services.AddTransient<GesekNoRangkaService>();
            services.AddTransient<HolidayService>();
            // TIE: START
            // services.AddTransient<IExcelExportHelperService, ExportHelperService>();
            // services.AddTransient<IExcelPackageExtension, ExcelPackageService>();
            // services.AddTransient<InspectionItemService>();
            // TIE: END
            services.AddTransient<KodeShiftService>();
            services.AddTransient<KonfigurasiGesekanService>();
            services.AddTransient<LeadTimeByService>();
            services.AddTransient<LocationService>();
            services.AddTransient<LocationTypeService>();
            services.AddTransient<LogisticVehicleService>();
            services.AddTransient<LogisticVendorService>();
            services.AddTransient<LogUploadDownloadService>();
            services.AddTransient<MaintenanceShiftKerjaService>();
            services.AddTransient<MaintenanceWaktuBreakService>();
            services.AddTransient<MasterCityLocationService>();
            services.AddTransient<MasterCompanyService>();
            services.AddTransient<MasterConfigurationPointPreBookVesselService>();
            services.AddTransient<MasterGroupDealerService>();
            services.AddTransient<MasterJenisService>();
            services.AddTransient<MasterLeadTimeLocationService>();
            services.AddTransient<MasterLeadTimeService>();
            services.AddTransient<MasterManufacturingService>();
            services.AddTransient<MasterModelSeriesService>();
            services.AddTransient<MasterModelService>();
            services.AddTransient<MasterPlafondService>();
            services.AddTransient<MasterProsesService>();
            services.AddTransient<MasterRegionAfiService>();
            services.AddTransient<MasterRitasePriceService>();
            services.AddTransient<MasterWarnaVehicleService>();
            // TIE: START
            // services.AddTransient<MCCPService>();
            // TIE: END
            services.AddTransient<MdpApiServices>();
            services.AddTransient<PDCConfigService>();
            services.AddTransient<PDILeadTimeConfigurationService>();
            // TIE: START
            // services.AddTransient<PenyesuaianTanggalProduksiService>();
            // services.AddTransient<PIODefaultLeadTimeConfigurationService>();
            // TIE: END
            services.AddTransient<PIOLineMasterService>();
            services.AddTransient<PlanningKalenderKerjaPolaBreakSemingguService>();
            services.AddTransient<PlanningKalenderKerjaPolaKerjaSemingguService>();
            services.AddTransient<PolaRangkaianTahapAkhirPenerapanService>();
            services.AddTransient<PolaRangkaianTahapAkhirService>();
            services.AddTransient<PolaRangkaianTahapAwalPenerapanService>();
            services.AddTransient<PolaRangkaianTahapAwalService>();
            services.AddTransient<RegionService>();
            services.AddTransient<ReportGesekanService>();
            services.AddTransient<RoleMenuService>();
            services.AddTransient<RoutingDictionaryDetailService>();
            services.AddTransient<RoutingDictionaryService>();
            services.AddTransient<RoutingProductionLeadTimeService>();
            services.AddTransient<SalesAreaService>();
            services.AddTransient<SerahTerimaGesekanService>();
            // TIE: START
            // services.AddTransient<SPUDefaultLeadTimeConfigurationService>();
            // TIE: END
            services.AddTransient<SPULineMasterService>();
            // TIE: START
            // services.AddTransient<TariffService>();
            // TIE: END
            services.AddTransient<UpdateFailedAttribute>();
            // TIE: START
            // services.AddTransient<UploadDCCPExcelService>();
            // TIE: END
            services.AddTransient<UploadDownloadService>();
            services.AddTransient<VesselArrivalService>();
            services.AddTransient<VesselDepartService>();
            services.AddTransient<DeliveryShippingScheduleService>();
            // TIE: START
            // services.AddTransient<MaintenanceKonfigurasiExportFileDccpService>();
            // TIE: END
            services.AddTransient<AFIRestriksiAreaService>();
        }

        private void AddStorageServices(IServiceCollection services)
        {
            services.AddHangfire(configuration =>
            {
                configuration.UseRedisStorage(Redis, new Hangfire.Redis.RedisStorageOptions
                {
                    Prefix = "TLS_HF_"
                });
            });

            services.AddDbContext<LogisticDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LogisticDb"), strategy =>
                {
                    strategy.EnableRetryOnFailure();
                });
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "TLS_";
            });

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddSerilog();
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            app.UseWhen(context => context.IsApiContext(), builder =>
            {
                builder.UseApiExceptionHandler(DebugMode);
            });

            app.UseWhen(context => context.IsApiContext() == false, builder =>
            {
                if (DebugMode)
                {
                    builder.UseDeveloperExceptionPage();
                }
                else
                {
                    builder.UseExceptionHandler("/Home/Error");
                }
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = AuthenticationService.CookieAuthenticationScheme,
                LoginPath = "/auth/login",
                LogoutPath = "/auth/logout",
                AccessDeniedPath = "/home/denied",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseStaticFiles();
            app.UseSession();

            app.UseHangfireServer();
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireDashboardAdministratorAuthorizationFilter() }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

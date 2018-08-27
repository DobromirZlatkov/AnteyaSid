namespace AnteyaSidOnContainers.WebApps.WebMVC
{
    using AnteyaSidOnContainers.BuildingBlocks.Resilience.Http;
    using AnteyaSidOnContainers.BuildingBlocks.Resilience.Http.Contracts;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.AutofacMidules;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.Mapping;
    using AnteyaSidOnContainers.WebApps.WebMVC.Infrastructure.Middlewares;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services;
    using AnteyaSidOnContainers.WebApps.WebMVC.Services.Contracts;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.ServiceFabric;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.IdentityModel.Tokens.Jwt;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            RegisterAppInsights(services);

            services
                .AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;

            services.AddKendo();

            services.AddSession();

            AutoMapperConfig.RegisterMappings();

            services.Configure<AppSettings>(Configuration);

            //By connecting here we are making sure that our service
            //cannot start until redis is ready. This might slow down startup,
            //but given that there is a delay on resolving the ip address
            //and then creating the connection it seems reasonable to move
            //that cost to startup instead of having the first request pay the
            //penalty.
            //services.AddSingleton<ConnectionMultiplexer>(sp =>
            //{
            //    var settings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
            //    var configuration = ConfigurationOptions.Parse(settings.ConnectionString, true);

            //    configuration.ResolveDns = true;

            //    return ConnectionMultiplexer.Connect(configuration);
            //});



            // Add application services.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            if (Configuration.GetValue<string>("UseResilientHttp") == bool.TrueString)
            {
                services.AddSingleton<IResilientHttpClientFactory, ResilientHttpClientFactory>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<ResilientHttpClient>>();
                    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

                    var retryCount = 6;
                    if (!string.IsNullOrEmpty(Configuration["HttpClientRetryCount"]))
                    {
                        retryCount = int.Parse(Configuration["HttpClientRetryCount"]);
                    }

                    var exceptionsAllowedBeforeBreaking = 5;
                    if (!string.IsNullOrEmpty(Configuration["HttpClientExceptionsAllowedBeforeBreaking"]))
                    {
                        exceptionsAllowedBeforeBreaking = int.Parse(Configuration["HttpClientExceptionsAllowedBeforeBreaking"]);
                    }

                    return new ResilientHttpClientFactory(logger, httpContextAccessor, exceptionsAllowedBeforeBreaking, retryCount);
                });
                services.AddSingleton<IHttpClient, ResilientHttpClient>(sp => sp.GetService<IResilientHttpClientFactory>().CreateResilientHttpClient());
            }
            else
            {
                services.AddSingleton<IHttpClient, StandardHttpClient>();
            }

            var useLoadTest = Configuration.GetValue<bool>("UseLoadTest");
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var callBackUrl = Configuration.GetValue<string>("CallBackUrl");

            // Add Authentication services
            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Authority = identityUrl.ToString();
                options.SignedOutRedirectUri = callBackUrl.ToString();
                options.ClientId = useLoadTest ? "mvctest" : "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = useLoadTest ? "code id_token token" : "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.RequireHttpsMetadata = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("catalog");
            });

            services.AddOptions();

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new ServicesModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();
            loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger("init").LogDebug($"Using PATH BASE '{pathBase}'");
                app.UsePathBase(pathBase);
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseKendo(env);

            if (Configuration.GetValue<bool>("UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();

            var log = loggerFactory.CreateLogger("identity");

            WebContextSeed.Seed(app, env, loggerFactory);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "defaultError",
                    template: "{controller=Error}/{action=Error}");
            });
        }

        private void RegisterAppInsights(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry(Configuration);
            var orchestratorType = Configuration.GetValue<string>("OrchestratorType");

            if (orchestratorType?.ToUpper() == "K8S")
            {
                // Enable K8s telemetry initializer
                services.EnableKubernetes();
            }
            if (orchestratorType?.ToUpper() == "SF")
            {
                // Enable SF telemetry initializer
                services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                    new FabricTelemetryInitializer());
            }
        }
    }
}

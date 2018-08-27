namespace AnteyaSidOnContainers.Services.Catalog.API
{
    using System;
    using System.Data.Common;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using AnteyaSidOnContainers.BuildingBlocks.EventBus.IntegrationEventLogEF;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.IntegrationEventLogEF.Services;
    using AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.Filters;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.RabbitMQ;
    using Microsoft.Extensions.Logging;
    using RabbitMQ.Client;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid.Abstractions;
    using AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.AnteyaSid;
    using AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.AutofacModules;
    using AnteyaSidOnContainers.Services.Catalog.Data;
    using AnteyaSidOnContainers.Services.Catalog.API.Application.IntegrationEvents.Events;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Swashbuckle.AspNetCore.Swagger;
    using System.Collections.Generic;
    using AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.Infrastructure;
    using AnteyaSidOnContainers.Services.Catalog.API.Auth.Server;

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
            services
                .AddRabbitMqPersistenConnection(Configuration)
                .AddEventBus(Configuration)
                .AddDatabase(Configuration)
                .AddKendo()
                .AddOptions()
                .Configure<CatalogSettings>(Configuration) // Setup settings class from settings file
                .AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp => (DbConnection c) => new IntegrationEventLogService(c))
                .AddSwaggerGen(options => // Add swagger documentation for the microservice
                {
                    options.DescribeAllEnumsAsStrings();
                    options.SwaggerDoc("v1", new Info
                    {
                        Title = "anteyaSidOnContainers - Catalog HTTP API",
                        Version = "v1",
                        Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                        TermsOfService = "Terms Of Service"
                    });

                    options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                    {
                        Type = "oauth2",
                        Flow = "implicit",
                        AuthorizationUrl = $"{Configuration.GetValue<string>("IdentityUrl")}/connect/authorize",
                        TokenUrl = $"{Configuration.GetValue<string>("IdentityUrl")}/connect/token",
                        Scopes = new Dictionary<string, string>()
                        {
                            { "catalog", "Catalog API" }
                        }
                    });

                    options.OperationFilter<AuthorizeCheckOperationFilter>();
                })
                .AddCustomAuthentication(Configuration)
                .AddCors(options =>  // Configure CORS
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                })
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddControllersAsServices();

            // configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());
            container.RegisterModule(new CatalogServicesModule());
            container.RegisterModule(new CatalogDataModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
         
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            //// IMPORTANT ! Configuring auth should be before configuring swagger because authentication doesn't work.
            ConfigureAuth(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/v1/{controller=Home}/{action=Index}/{id?}");
            });

            var pathBase = Configuration["PATH_BASE"];

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Catalog.API V1");
                 //  c.ConfigureOAuth2("Microsoft.eShopOnContainers.Mobile.Shopping.HttpAggregatorwaggerui", "", "", "Purchase BFF Swagger UI");
                   c.OAuthClientId("catalogswaggerui");
                   c.OAuthAppName("Catalog Swagger UI");
               });
             
            ConfigureEventBus(app);
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration.GetValue<bool>("UseLoadTest"))
            {
                app.UseMiddleware<ByPassAuthMiddleware>();
            }

            app.UseAuthentication();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<CatalogItemUpdateIntegrationEvent, IIntegrationEventHandler<CatalogItemUpdateIntegrationEvent>>();
            eventBus.Subscribe<CatalogItemDeleteIntegrationEvent, IIntegrationEventHandler<CatalogItemDeleteIntegrationEvent>>();
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = "catalog";
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration.GetValue<string>("SubscriptionClientName");

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration.GetValue<string>("EventBusRetryCount")))
                {
                    retryCount = int.Parse(configuration.GetValue<string>("EventBusRetryCount"));
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddEntityFrameworkNpgsql().AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(configuration.GetValue<string>("NpgConnectionString"),
                  npgsqlOptionsAction: npgsqlOption =>
                  {
                      npgsqlOption.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                      npgsqlOption.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(2), errorCodesToAdd: null);
                  }));

            services.AddEntityFrameworkNpgsql().AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseNpgsql(configuration.GetValue<string>("NpgConnectionString"),
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(2), errorCodesToAdd: null);
                    });
            });

            return services;
        }

        public static IServiceCollection AddRabbitMqPersistenConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(configuration.GetValue<string>("EventBusConnectionUrl"))
                };

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration.GetValue<string>("EventBusRetryCount")))
                {
                    retryCount = int.Parse(configuration.GetValue<string>("EventBusRetryCount"));
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            return services;
        }
    }
}

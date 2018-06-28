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
    using AnteyaSidOnContainers.Services.Catalog.API.IntegrationEvents.Events;
    using AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.AutofacModules;
    using AnteyaSidOnContainers.Services.Catalog.Data;

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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddControllersAsServices();
            
            services.AddEntityFrameworkNpgsql().AddDbContext<CatalogDbContext>(options =>
                options.UseNpgsql(Configuration["NpgConnectionString"],
                  npgsqlOptionsAction: npgsqlOption =>
                  {
                      npgsqlOption.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                      npgsqlOption.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(2), errorCodesToAdd: null);
                  }));


            services.AddEntityFrameworkNpgsql().AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseNpgsql(Configuration["NpgConnectionString"],
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(2), errorCodesToAdd: null);
                    });
            });

            // Setup settings class from settings file
            services.Configure<CatalogSettings>(Configuration);

            services.AddKendo();

            // Setup event bus connection
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(Configuration["EventBusConnectionUrl"])
                };

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            RegisterEventBus(services);

            services.AddOptions();

            // Add swagger documentation for the microservice
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "anteyaSidOnContainers - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample",
                    TermsOfService = "Terms Of Service"
                });
            });

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));


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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigureEventBus(app);
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<CatalogItemCreatedIntegrationEvent, IIntegrationEventHandler<CatalogItemCreatedIntegrationEvent>>();
        }
    }
}

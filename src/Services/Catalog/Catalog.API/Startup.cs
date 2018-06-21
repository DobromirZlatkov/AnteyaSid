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
    using AnteyaSidOnContainers.Services.Catalog.API.Data;
    using AnteyaSidOnContainers.Services.Catalog.API.Infrastructure.Filters;

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

            services.AddEntityFrameworkNpgsql().AddDbContext<CatalogContext>(options =>
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

            //services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

            var container = new ContainerBuilder();
            container.Populate(services);

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
        }
    }
}

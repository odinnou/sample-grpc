using AutoMapper;
using Common.API.Configuration;
using Common.API.Documentation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Product.API.Configuration;
using Product.API.Grpc;
using Product.API.Infrastructure;
using Product.API.Infrastructure.Filters;
using System;
using System.Reflection;

namespace Product.API
{
    public class Startup
    {
        public const string TEST_ENVIRONMENT = "test";

        public Startup(IConfiguration configuration, IWebHostEnvironment appEnv)
        {
            Configuration = configuration;
            CurrentEnvironment = appEnv;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ProductApiSettings>(Configuration.GetSection(nameof(ProductApiSettings)));

            ProductApiSettings recipeApiSettings = new ProductApiSettings();
            Configuration.GetSection(nameof(ProductApiSettings)).Bind(recipeApiSettings);

            services.AddGrpc();
            services.AddCors();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));

            }).AddNewtonsoftJson();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(Assembly.Load(typeof(Startup).Assembly.GetName().Name!));
            services.AddSwagger("1.0.0");
            services.AddHealthChecks();
            services.AddDependencies(recipeApiSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ProductApiSettings> settingsOptionsRecipe, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            ProductApiSettings recipeApiSettings = settingsOptionsRecipe.Value;

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            InitializeDatabase(app, webHostEnvironment);

            if (recipeApiSettings.EnableSwagger)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfig("1.0.0");
            }

            app.UseRouting();
            app.UseCors(builder =>
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders(CommonController.TOTAL_COUNT_HEADER));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ProductService>();
                endpoints.MapHealthChecks("/health");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (!TEST_ENVIRONMENT.Equals(webHostEnvironment.EnvironmentName, StringComparison.OrdinalIgnoreCase))
            {
                using IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                serviceScope.ServiceProvider.GetRequiredService<ProductContext>().Database.Migrate();
            }
        }
    }
}

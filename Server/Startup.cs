using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Configuration;
using Server.Controllers;
using Server.Documentation;
using Server.Grpc;
using Server.Infrastructure;
using Server.Infrastructure.Filters;
using System.Reflection;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment appEnv)
        {
            Configuration = configuration;
            CurrentEnvironment = appEnv;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            AppSettings appSettings = new AppSettings();
            Configuration.GetSection(nameof(AppSettings)).Bind(appSettings);

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
            services.AddDependencies(appSettings);
        }

        public void Configure(IApplicationBuilder app, IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            InitializeDatabase(app);

            app.UseDeveloperExceptionPage();
            app.UseSwaggerConfig("1.0.0");

            app.UseRouting();
            app.UseCors(builder =>
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders(ProductController.TOTAL_COUNT_HEADER));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ProductService>();
                endpoints.MapHealthChecks("/health");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<ProductContext>().Database.Migrate();
        }
    }
}

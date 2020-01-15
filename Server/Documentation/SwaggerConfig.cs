using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using System.Reflection;

namespace Server.Documentation
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, string version)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{version}", new OpenApiInfo
                {
                    Version = $"v{version}",
                    Title = $"gRPC Server Sample {Assembly.GetEntryAssembly()!.GetName().Name}",
                    Description = $"gRPC Server Sample {Assembly.GetEntryAssembly()!.GetName().Name}"
                });

                var xmlPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "xml");
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, string version)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"gRPC Server Sample {Assembly.GetEntryAssembly()!.GetName().Name} v{version}");
                c.RoutePrefix = string.Empty;
                c.DefaultModelExpandDepth(3);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(-1);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.List);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
            });

            return app;
        }
    }
}

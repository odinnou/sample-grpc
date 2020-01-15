using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Server.Infrastructure;
using Server.Repositories;
using Server.Repositories.Interfaces;
using Server.UseCases;
using Server.UseCases.Interfaces;

namespace Server.Configuration
{
    public static class DependencyConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, AppSettings appSettings)
        {
            #region Database
            services.AddDbContext<ProductContext>(options => options.UseLazyLoadingProxies().UseNpgsql(appSettings.DbConnection).UseSnakeCaseNamingConvention());
            #endregion

            #region Services
            services.AddTransient<IProductFetcher, ProductFetcher>();
            #endregion

            #region Repositories
            services.AddTransient<IProductRepository, ProductRepository>();
            #endregion

            return services;
        }
    }
}

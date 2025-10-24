using ECommerce.ShareLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add database connectivity
            //Add authentication scheme
            SharedServiceContainer.AddShareServices<ProductDbContext>(services, configuration, configuration["MySerilog:FileName"]!);

            //Create Dependency Injection (DI)
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as:
            //Global Exception: handler external errors
            //Listen to Only Api Gateway: blocks all outside calls
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}

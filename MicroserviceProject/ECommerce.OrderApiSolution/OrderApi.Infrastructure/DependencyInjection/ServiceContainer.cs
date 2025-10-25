using ECommerce.ShareLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Database Connectivity
            //Add authentication scheme
            SharedServiceContainer.AddShareServices<OrderDbContext>(services, configuration, configuration["MySerilog:FileName"]!);

            //Create Dependency Injection
            services.AddScoped<IOrder, OrderRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register  middleware such as:
            //Global Exceptipn -> handle external errors
            //ListenToApiGateway -> block all outside API calls
            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}

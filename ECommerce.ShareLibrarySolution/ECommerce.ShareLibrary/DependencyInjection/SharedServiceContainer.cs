using ECommerce.ShareLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ECommerce.ShareLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddShareServices<TContext>(this IServiceCollection services, IConfiguration configuration, string fileName) where TContext : DbContext
        {
            //Add Generic Database Context
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("eCommerceConnection"),
                    sqlserverOption => sqlserverOption.EnableRetryOnFailure()
                    ));

            //confiure serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level: u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day).CreateLogger();

            //Add JWT Authentication Scheme
            services.AddJWTAuthenticationScheme(configuration);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            //Use global exceptipon
            app.UseMiddleware<GlobalException>();

            //Register middleware to block all outsides API calls
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}

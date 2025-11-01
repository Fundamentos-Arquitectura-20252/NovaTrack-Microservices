using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SharedKernel.Infrastructure.Middleware;

namespace SharedKernel.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSharedMiddleware(this IApplicationBuilder app)
        {
            // Logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            // Global exception handler
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }

        public static IApplicationBuilder UseSharedInfrastructure(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSharedMiddleware();

            // Aquí podrías añadir otras configuraciones globales,
            // como inicializar colas, eventos o health checks.
            
            return app;
        }
    }
}
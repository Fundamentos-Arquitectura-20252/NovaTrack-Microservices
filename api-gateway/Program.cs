using Microsoft.AspNetCore.HttpOverrides; // Necesario para AWS/Azure/Nginx
using Steeltoe.Discovery.Eureka;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servicios ---
// Cliente de Eureka para encontrar los microservicios
builder.Services.AddEurekaDiscoveryClient();

// Tu proveedor de configuración personalizado (el archivo EurekaProxyConfigProvider.cs)
builder.Services.AddSingleton<IProxyConfigProvider, EurekaProxyConfigProvider>();

// YARP
builder.Services.AddReverseProxy();

// Swagger Generator (necesario para pintar la UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. Middleware para Producción (CRÍTICO PARA AWS/AZURE) ---
// Esto permite que el Gateway sepa si la petición original venía por HTTPS
// aunque el Load Balancer de AWS se la pase por HTTP. Evita errores de "Mixed Content".
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// --- 3. Configuración de Swagger ---
app.UseSwagger(); // Genera el JSON propio del Gateway
app.UseSwaggerUI(options =>
{
    // TRUCO: Accedemos a la configuración de YARP en memoria para ver qué rutas existen
    // Esto es lo que llena el Dropdown de la derecha en Swagger.
    var configProvider = app.Services.GetRequiredService<IProxyConfigProvider>();
    var config = configProvider.GetConfig(); 

    if (config != null && config.Routes.Any())
    {
        foreach (var route in config.Routes)
        {
            var name = route.RouteId; // ej: "iamapi"
            // Crea la opción en el menú desplegable
            options.SwaggerEndpoint($"/{name}/swagger/v1/swagger.json", $"{name} API");
        }
    }
    
    // Abre Swagger en la raíz (localhost:8080) en lugar de localhost:8080/swagger
    options.RoutePrefix = string.Empty; 
});

// --- 4. Activar YARP ---
app.MapReverseProxy();

app.Run();
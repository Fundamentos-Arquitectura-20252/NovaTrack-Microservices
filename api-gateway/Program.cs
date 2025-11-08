using Steeltoe.Discovery.Eureka;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Steeltoe Eureka client
builder.Services.AddEurekaDiscoveryClient();

// YARP + Eureka dynamic config
builder.Services.AddSingleton<IProxyConfigProvider, EurekaProxyConfigProvider>();
builder.Services.AddReverseProxy(); // sin LoadFromConfig

var app = builder.Build();

app.MapReverseProxy();

app.Run();
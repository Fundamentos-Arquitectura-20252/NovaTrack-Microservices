using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Reflection;
using Analytics.API.Infrastructure.Extensions;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

// Controllers & JSON config
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddHealthChecks();

// MediatR for CQRS
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// Register external services to other microservices
builder.Services.AddAnalyticsServices(); // This adds FleetServiceClient, PersonnelServiceClient, etc

// Eureka Discovery
builder.Services.AddEurekaDiscoveryClient();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
));

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
namespace FleetManagement.API.Interfaces.REST.Middleware;
using System.Net;
using System.Text.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            Console.WriteLine($"EL ERROR ES: {error.ToString()}");
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                // Error de validación de Dominio (ej. Formato de placa mal, Año inválido)
                case ArgumentException: 
                    response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                    
                // Error de lógica de negocio (ej. Placa duplicada)
                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                    break;
                    
                default:
                    // Error real del servidor (bugs inesperados)
                    response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error.Message });
            await response.WriteAsync(result);
        }
    }
}
using Analytics.API.Application.External.Services;
using Steeltoe.Common.Discovery;

namespace Analytics.API.Infrastructure.Services.External;

public class PersonnelServiceClient : IPersonnelServiceClient
{
    private readonly HttpClient _httpClient;
    private const string SERVICE_NAME = "PERSONNEL.API"; // Nombre tal como está registrado en Eureka

    public PersonnelServiceClient(IDiscoveryClient discoveryClient, HttpClient httpClient)
    {
        _httpClient = httpClient;
        InitializeAsync(discoveryClient).GetAwaiter().GetResult();
    }
    
    private async Task InitializeAsync(IDiscoveryClient discoveryClient)
    {
        // Buscar instancia del servicio en Eureka
        var instances = await discoveryClient.GetInstancesAsync(SERVICE_NAME, CancellationToken.None);
        var instance = instances.FirstOrDefault();

        if (instance == null)
            throw new Exception($"{SERVICE_NAME} no encontrado en Eureka");

        _httpClient.BaseAddress = instance.Uri;
    }

    public async Task<int> GetActiveDriversCountAsync()
    {
        try
        {
            // Llamada al endpoint del microservicio Personnel
            var result = await _httpClient.GetFromJsonAsync<int>("/api/drivers/active/count");

            return result;
        }
        catch (Exception ex)
        {
            // Loguear si quieres
            Console.WriteLine($"Error llamando a PersonnelService: {ex.Message}");
            return 0; // Valor por defecto si el servicio está caído
        }
    }
}
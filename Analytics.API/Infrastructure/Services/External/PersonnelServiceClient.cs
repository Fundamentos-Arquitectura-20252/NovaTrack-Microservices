using Analytics.API.Application.External.Services;
using Steeltoe.Common.Discovery;

namespace Analytics.API.Infrastructure.Services.External;

public class PersonnelServiceClient : IPersonnelServiceClient
{
    private readonly HttpClient _http;

    public PersonnelServiceClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<int> GetActiveDriversCountAsync()
    {
        return await _http.GetFromJsonAsync<int>("/api/drivers/active/count");
    }
}

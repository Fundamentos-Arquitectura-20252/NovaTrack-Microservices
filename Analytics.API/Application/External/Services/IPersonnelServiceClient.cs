namespace Analytics.API.Application.External.Services;

public interface IPersonnelServiceClient
{
    Task<int> GetActiveDriversCountAsync();
}
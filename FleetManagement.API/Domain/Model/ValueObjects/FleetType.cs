using System.Text.RegularExpressions;

// FleetType.cs
namespace FleetManagement.API.Domain.Model.ValueObjects
{
    public enum FleetType
    {
        Primary,
        Secondary,
        External,
        Rental
    }
}
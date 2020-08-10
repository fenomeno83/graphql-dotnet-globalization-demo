using GraphQL.Globalization.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace GraphQL.Globalization.Interfaces
{
    public interface IInfrastructureService
    {
        IStringLocalizer<Resources> Localizer { get; }
        IConfiguration Configuration { get; }
        ILogService Log { get; }
        IEnumsManager EnumsManager { get; }

    }
}

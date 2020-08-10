using GraphQL.Globalization.Common;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Services
{
    public class InfrastructureService : IInfrastructureService
    {
        public IStringLocalizer<Resources> Localizer { get; }
        public IConfiguration Configuration { get; }
        public ILogService Log { get; }
        public IEnumsManager EnumsManager { get; }

        public InfrastructureService(IStringLocalizer<Resources> localizer,
            IConfiguration configuration, ILogService log, IEnumsManager enumsManager)
        {
            Localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Log = log ?? throw new ArgumentNullException(nameof(log));
            EnumsManager = enumsManager ?? throw new ArgumentNullException(nameof(enumsManager));

        }
    }
}

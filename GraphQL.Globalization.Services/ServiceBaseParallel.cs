using GraphQL.Globalization.Data;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Services
{
    //use this if we have parallel execution
    public class ServiceBaseParallel
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogService _log;
        protected readonly IStringLocalizer<Resources> _localizer;
        protected readonly IEnumsManager _enumsManager;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly Func<ApplicationDbContext> _dbfunc;

        public ServiceBaseParallel(IInfrastructureService infrastructure, IHttpContextAccessor httpContextAccessor, Func<ApplicationDbContext> dbfunc)
        {
            _configuration = infrastructure.Configuration;
            _log = infrastructure.Log;
            _localizer = infrastructure.Localizer;
            _enumsManager = infrastructure.EnumsManager;
            _httpContextAccessor = httpContextAccessor;
            _dbfunc = dbfunc;

        }
    }
}

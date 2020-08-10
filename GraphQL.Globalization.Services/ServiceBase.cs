using GraphQL.Globalization.Common;
using GraphQL.Globalization.Data;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GraphQL.Globalization.Services
{
    public class ServiceBase
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogService _log;
        protected readonly IStringLocalizer<Resources> _localizer;
        protected readonly IEnumsManager _enumsManager;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ApplicationDbContext _db;

        public ServiceBase(IInfrastructureService infrastructure, IHttpContextAccessor httpContextAccessor, ApplicationDbContext db)
        {
            _configuration = infrastructure.Configuration;
            _log = infrastructure.Log;
            _localizer = infrastructure.Localizer;
            _enumsManager = infrastructure.EnumsManager;
            _httpContextAccessor = httpContextAccessor;
            _db = db;

        }


    }
}


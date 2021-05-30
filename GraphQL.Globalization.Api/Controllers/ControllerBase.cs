using GraphQL;
using GraphQL.Types;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IDocumentExecuter _documentExecuter;
        protected readonly ISchema _schema;
        protected readonly ILogService _logService;
        protected readonly IConfiguration _configuration;
        protected readonly IDocumentWriter _documentWriter;


        public BaseController(ISchema schema, IDocumentExecuter documentExecuter, ILogService logService, IConfiguration configuration, IDocumentWriter documentWriter)
        {
            _logService = logService;
            _configuration = configuration;
            _schema = schema;
            _documentExecuter = documentExecuter;
            _documentWriter = documentWriter;

        }

    }
}

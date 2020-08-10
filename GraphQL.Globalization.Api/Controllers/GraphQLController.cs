using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.Globalization.Common.Extensions;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Api.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : ControllerBase
    {

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogService logService, IConfiguration configuration)
            : base(schema, documentExecuter, logService, configuration)
        {
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GraphQLQuery query)
        {

            Stopwatch timer = Stopwatch.StartNew();

            string correlation = Guid.NewGuid().ToString();
            HttpContext.SetCorrelation(correlation);

            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var result = await _documentExecuter.ExecuteAsync(new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = query.Variables.ToInputs()
            });

            timer.Stop();

            result.Errors?.ToList().ForEach(err =>
            {
                result.Errors.Add(new ExecutionError(err.ToString()));
                _logService.WriteLog(LogLevelL4N.ERROR, $"ID: {correlation}\n{err.ToString()}");
            });

            var groupItem = (result?.Data?.GetValue() as Dictionary<string, object>)?.FirstOrDefault();
            string groupName = groupItem?.Key ?? string.Empty;

            var methodItem = (groupItem?.Value as Dictionary<string, object>)?.FirstOrDefault();
            string methodName = methodItem?.Key ?? string.Empty;

            bool.TryParse(_configuration["Log4NetConfigFile:LogBodyRequest"], out bool logBodyRequest);

            if (result.Errors?.Count > 0)
            {

                if (logBodyRequest)
                    _logService.WriteLog(LogLevelL4N.ERROR, string.Format(ApiMessages.ApiErrorLogWithBodyRequest, correlation, $"{groupName}/{methodName}", timer.ElapsedMilliseconds.ToString("N0"), HttpContext.GetBodyString() ?? string.Empty));
                else
                    _logService.WriteLog(LogLevelL4N.ERROR, string.Format(ApiMessages.ApiErrorLog, correlation, $"{groupName}/{methodName}", timer.ElapsedMilliseconds.ToString("N0")));


                return BadRequest(result);
            }
            else
            {
                bool.TryParse(_configuration["Log4NetConfigFile:Automatic"], out bool autolog);

                if (autolog)
                {
                    if (logBodyRequest)
                        _logService.WriteLog(LogLevelL4N.INFO, string.Format(ApiMessages.ApiMethodLogWithBodyRequest, correlation, $"{groupName}/{methodName}", timer.ElapsedMilliseconds.ToString("N0"), HttpContext.GetBodyString() ?? string.Empty));
                    else
                        _logService.WriteLog(LogLevelL4N.INFO, string.Format(ApiMessages.ApiMethodLog, correlation, $"{groupName}/{methodName}", timer.ElapsedMilliseconds.ToString("N0")));

                }

                return Ok(result);

            }

        }
    }

    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
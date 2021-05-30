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
using GraphQL.NewtonsoftJson;
using System.Net;

namespace GraphQL.Globalization.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphQLController : BaseController
    {

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, ILogService logService, IConfiguration configuration, IDocumentWriter documentWriter)
            : base(schema, documentExecuter, logService, configuration, documentWriter)
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
                result.Errors.Add(new ExecutionError(string.IsNullOrWhiteSpace(err.InnerException?.Message) ? err.ToString() : err.InnerException.Message));
                _logService.WriteLog(LogLevelL4N.ERROR, $"ID: {correlation}\n{err.ToString()}");
            });

            List<string> methods = new List<string>();

            bool log = true;
            var root = (result?.Data) as Execution.RootExecutionNode;
            var groupItems = root?.SubFields;
            if (groupItems != null && groupItems.Length > 0)
            {
                foreach (var g in groupItems)
                {
                    var node = g as Execution.ObjectExecutionNode;
                    string groupName = node.Name ?? string.Empty;

                    if (groupName == GraphQLConst.Schema)
                        log = false;

                    var methodItems = node?.SubFields;

                    if (methodItems != null && methodItems.Length > 0)
                    {
                        foreach (var m in methodItems)
                        {
                            string methodName = m.Name ?? string.Empty;

                            methods.Add($"{groupName}/{methodName}");
                        }
                    }
                }
            }


            methods = methods.OrderBy(x => x).ToList();

            string label = string.Join("-", methods);

            bool.TryParse(_configuration["Log4NetConfigFile:LogBodyRequest"], out bool logBodyRequest);

            if (result.Errors?.Count > 0)
            {

                if (logBodyRequest)
                    _logService.WriteLog(LogLevelL4N.ERROR, string.Format(ApiMessages.ApiErrorLogWithBodyRequest, correlation, label, timer.ElapsedMilliseconds.ToString("N0"), HttpContext.GetBodyString() ?? string.Empty));
                else
                    _logService.WriteLog(LogLevelL4N.ERROR, string.Format(ApiMessages.ApiErrorLog, correlation, label, timer.ElapsedMilliseconds.ToString("N0")));


                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Response.ContentType = "application/json";
                await _documentWriter.WriteAsync(Response.Body, result, HttpContext.RequestAborted);
                return new EmptyResult();
            }
            else
            {
                bool.TryParse(_configuration["Log4NetConfigFile:Automatic"], out bool autolog);

                if (autolog && log)
                {
                    if (logBodyRequest)
                        _logService.WriteLog(LogLevelL4N.INFO, string.Format(ApiMessages.ApiMethodLogWithBodyRequest, correlation, label, timer.ElapsedMilliseconds.ToString("N0"), HttpContext.GetBodyString() ?? string.Empty));
                    else
                        _logService.WriteLog(LogLevelL4N.INFO, string.Format(ApiMessages.ApiMethodLog, correlation, label, timer.ElapsedMilliseconds.ToString("N0")));

                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                Response.ContentType = "application/json";
                await _documentWriter.WriteAsync(Response.Body, result, HttpContext.RequestAborted);
                return new EmptyResult();

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
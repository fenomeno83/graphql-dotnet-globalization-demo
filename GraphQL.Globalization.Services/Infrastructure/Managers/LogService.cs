using GraphQL.Globalization.Interfaces;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Services.Infrastructure.Managers
{
    
    public class LogService : ILogService
    {
        private readonly ILog logger;

        public LogService()
        {
            logger = LogManager.GetLogger(typeof(LogService));
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        public void WriteLog(LogLevelL4N logLevel, string log)
        {

            if (logLevel.Equals(LogLevelL4N.DEBUG))
            {
                logger.Info(log);
            }

            else if (logLevel.Equals(LogLevelL4N.ERROR))
            {
                logger.Error(log);
            }

            else if (logLevel.Equals(LogLevelL4N.FATAL))
            {
                logger.Fatal(log);
            }

            else if (logLevel.Equals(LogLevelL4N.INFO))
            {
                logger.Info(log);
            }

            else if (logLevel.Equals(LogLevelL4N.WARN))
            {
                logger.Warn(log);
            }
            else if (logLevel.Equals(LogLevelL4N.TRACE))
            {
                logger.Info(log);
            }
        }

    }
}

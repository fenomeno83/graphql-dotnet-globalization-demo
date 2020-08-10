using System;
using System.Collections.Generic;
using System.Text;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Interfaces
{
    
    public interface ILogService
    {
        void WriteLog(LogLevelL4N logLevel, string log);

    }
}

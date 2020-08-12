using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GraphQL.Globalization.Interfaces
{
    public interface IContextService
    {
        ValidationContext GetValidationContext<T>(T request);
        IServiceScope CreateScope();
    }
}

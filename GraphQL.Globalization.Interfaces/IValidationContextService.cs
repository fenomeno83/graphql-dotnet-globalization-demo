using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GraphQL.Globalization.Interfaces
{
    public interface IValidationContextService
    {
        ValidationContext GetValidationContext<T>(T request);
    }
}

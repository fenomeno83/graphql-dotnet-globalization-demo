using GraphQL.Globalization.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GraphQL.Globalization.Services.Infrastructure.Managers
{
    public class ValidationContextService : IValidationContextService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationContextService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ValidationContext GetValidationContext<T>(T request)
        {
            return new ValidationContext(request,
               _serviceProvider,
               null);
        }
    }
}

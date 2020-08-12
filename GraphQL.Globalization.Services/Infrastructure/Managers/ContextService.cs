using GraphQL.Globalization.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GraphQL.Globalization.Services.Infrastructure.Managers
{
    public class ContextService : IContextService
    {
        private readonly IServiceProvider _serviceProvider;

        public ContextService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ValidationContext GetValidationContext<T>(T request)
        {
            return new ValidationContext(request,
               _serviceProvider,
               null);
        }

        public IServiceScope CreateScope()
        {
            return _serviceProvider.CreateScope();
        }
    }
}

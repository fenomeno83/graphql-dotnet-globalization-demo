using GraphQL.Globalization.Common.Extensions;
using GraphQL.Globalization.Entities;
using GraphQL.Globalization.Entities.Extensions;
using GraphQL.Globalization.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQL.Globalization.Api.GraphQLSchema.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static IServiceScope CreateScope(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IContextService>().CreateScope();
        }

        public static void ValidateScopes(this IHttpContextAccessor httpContextAccessor, params string[] scopes)
        {
            httpContextAccessor.HttpContext.ValidateScopes(scopes);
        }
        public static void Validate<T>(this IHttpContextAccessor httpContextAccessor, T model)
        {
            var validationContextService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IContextService>();
            var validationContext = validationContextService.GetValidationContext(model);

            IStringLocalizer<Resources> localizer = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<Resources>>();

            var invalidFields = ValidateModel(model, validationContext, localizer);
            if (invalidFields != null && invalidFields.Count > 0)
            {
                throw new System.Exception(string.Format(localizer["notvalid_model"], invalidFields.Aggregate((curr, next) => $"{curr}\n {next}")));
            }
        }

        private static List<string> ValidateModel<T>(T model, ValidationContext validationContext, IStringLocalizer<Resources> localizer)
        {
            var invalidFields = new List<string>();


            if (model == null)
                return invalidFields;


            Type myType = model.GetType();
            var classAttr = myType.GetCustomAttributes(true);

            if (classAttr != null)
            {
                ValidationAttribute[] validatorAttrs = classAttr
                    .Where(a => a is ValidationAttribute)
                    .Select(a => (ValidationAttribute)a).ToArray();

                foreach (var attr in validatorAttrs)
                {
                    ValidationResult res = null;
                    switch (attr.GetType().Name)
                    {

                        case nameof(AllOrAtLeastOnePropertyRequiredAttribute):
                            res = (attr as AllOrAtLeastOnePropertyRequiredAttribute).IsValidPublic(model, validationContext);
                            break;
                        case nameof(AllOrAtLeastOnePropertyRequiredIfAttribute):
                            res = (attr as AllOrAtLeastOnePropertyRequiredIfAttribute).IsValidPublic(model, validationContext);
                            break;

                        default:
                            break;

                    }


                    if (res != null)
                        invalidFields.Add(res.ErrorMessage);


                }
            }

            var properties = model.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (properties != null && properties.Length > 0)
            {
                foreach (var prop in properties)
                {
                    var attrs = prop.GetCustomAttributes(true);
                    if (attrs != null)
                    {
                        var val = prop.GetValue(model);
                        ValidationAttribute[] validatorAttrs = attrs
                            .Where(a => a is ValidationAttribute)
                            .Select(a => (ValidationAttribute)a).ToArray();

                        foreach (var attr in validatorAttrs)
                        {
                            bool isExtension = true;
                            ValidationResult res = null;
                            switch (attr.GetType().Name)
                            {
                                case nameof(RequiredIfAttribute):
                                    res = (attr as RequiredIfAttribute).IsValidPublic(val, prop.Name, validationContext);
                                    break;
                                case nameof(GreaterThanAttribute):
                                    res = (attr as GreaterThanAttribute).IsValidPublic(val, prop.Name, validationContext);
                                    break;
                                case nameof(RangeIfAttribute):
                                    res = (attr as RangeIfAttribute).IsValidPublic(val, prop.Name, validationContext);
                                    break;
                                case nameof(ValidEnumAttribute):
                                    res = (attr as ValidEnumAttribute).IsValidPublic(val, prop.Name, validationContext);
                                    break;
                                default:
                                    isExtension = false;
                                    break;

                            }

                            if (isExtension)
                            {
                                if (res != null)
                                    invalidFields.Add(res.ErrorMessage);

                            }
                            else
                            {

                                bool thisFieldIsValid = attr.IsValid(val);
                                if (!thisFieldIsValid)
                                {

                                    if (localizer != null)
                                        attr.ErrorMessage = localizer[attr.ErrorMessage];

                                    invalidFields.Add(attr.FormatErrorMessage(prop.Name));
                                }
                            }
                        }
                    }
                }
            }

            return invalidFields;
        }

    }
}

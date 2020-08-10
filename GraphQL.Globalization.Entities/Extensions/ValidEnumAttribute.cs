using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Globalization.Entities.Extensions
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        private String OriginalPropertyName { get; set; }

        public ValidationResult IsValidPublic(object value, string property, ValidationContext validationContext)
        {
            OriginalPropertyName = property;

            return IsValid(value, validationContext);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
                return ValidationResult.Success;

            var type = value.GetType();
            if (!type.IsEnum //don't activate validation if isn't enum
                || Enum.IsDefined(type, value)
                )
                return ValidationResult.Success;
            else
            {
                var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

                string errorMessage = localizer == null ? null : localizer[ErrorMessageString];
                if (string.IsNullOrWhiteSpace(errorMessage) || errorMessage == ErrorMessageString)
                    errorMessage = DefaultValidationMessage.ValidEnum;

                errorMessage = string.Format(errorMessage, string.IsNullOrWhiteSpace(OriginalPropertyName) ? validationContext.DisplayName : OriginalPropertyName);


                return new ValidationResult(errorMessage);
            }
        }
    }
}

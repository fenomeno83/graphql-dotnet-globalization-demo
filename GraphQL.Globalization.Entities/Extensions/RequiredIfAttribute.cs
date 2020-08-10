using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;


namespace GraphQL.Globalization.Entities.Extensions
{
    public class RequiredIfAttribute : RequiredAttribute
    {
        private String OriginalPropertyName { get; set; }
        private String PropertyName { get; set; }
        private Object[] DesiredValue { get; set; }

        public RequiredIfAttribute(String propertyName, params Object[] desiredvalue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
        }
        public ValidationResult IsValidPublic(object value, string property, ValidationContext validationContext)
        {
            OriginalPropertyName = property;

            return IsValid(value, validationContext);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;


            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (DesiredValue == null) //fixes case when the only value passed that activate validation is a null value
                DesiredValue = new object[] { null };

            foreach (var d in DesiredValue)
            {
                if ((proprtyvalue == null ? null : proprtyvalue.ToString()) == (d == null ? null : d.ToString()))
                {
                    var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

                    string errorMessage = localizer == null ? null : localizer[ErrorMessageString];
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                        ErrorMessage = errorMessage;

                    ErrorMessage = string.Format(ErrorMessage, string.IsNullOrWhiteSpace(OriginalPropertyName) ? validationContext.DisplayName : OriginalPropertyName);

                    ValidationResult result = base.IsValid(value, validationContext);
                    return result;
                }
            }


            return ValidationResult.Success;
        }

    }
}

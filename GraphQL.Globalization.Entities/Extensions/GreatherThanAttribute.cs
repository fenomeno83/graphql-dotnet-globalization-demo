using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GraphQL.Globalization.Entities.Extensions
{
    public class GreaterThanAttribute : ValidationAttribute
    {

        public GreaterThanAttribute(string otherProperty, bool equalAccepted)
        {
            OtherProperty = otherProperty;
            EqualAccepted = equalAccepted;
        }

        private string OriginalPropertyName { get; set; }
        public string OtherProperty { get; set; }
        public bool EqualAccepted { get; set; }

        public string FormatErrorMessage(string name, string otherName, IStringLocalizer<Resources> localizer)
        {

            string errorMessage = localizer == null ? null : localizer[ErrorMessageString];
            if (string.IsNullOrWhiteSpace(errorMessage) || errorMessage == ErrorMessageString)
                errorMessage = EqualAccepted ? DefaultValidationMessage.GreatherEqualsThan : DefaultValidationMessage.GreatherThan;

            return string.Format(errorMessage, name, otherName);
        }

        public ValidationResult IsValidPublic(object value, string property, ValidationContext validationContext)
        {
            OriginalPropertyName = property;
            return IsValid(value, validationContext);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);


            if (firstComparable != null && secondComparable != null)
            {
                if ((!EqualAccepted ? true : firstComparable.ToString() != secondComparable.ToString()) && firstComparable.CompareTo(secondComparable) < 1)
                {
                    object obj = validationContext.ObjectInstance;

                    var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

                    return new ValidationResult(
                        FormatErrorMessage(string.IsNullOrWhiteSpace(OriginalPropertyName) ? validationContext.DisplayName : OriginalPropertyName, OtherProperty, localizer));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {

            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }

}

using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Entities.Extensions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AllOrAtLeastOnePropertyRequiredAttribute : RequiredAttribute
    {
        private string[] PropertyList { get; set; }
        private AllOrAtLeastOneRequiredType AllOrAtLeastOneRequiredType { get; set; }

        public AllOrAtLeastOnePropertyRequiredAttribute(AllOrAtLeastOneRequiredType allOrAtLeastOneRequiredType, params string[] propertyList)
        {
            this.PropertyList = propertyList;
            this.AllOrAtLeastOneRequiredType = allOrAtLeastOneRequiredType;
        }

        public ValidationResult IsValidPublic(object value, ValidationContext validationContext)
        {
            return IsValid(value, validationContext);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) //don't activate if class is null
                return ValidationResult.Success;

            PropertyInfo propertyInfo;

            switch (AllOrAtLeastOneRequiredType)
            {
                case AllOrAtLeastOneRequiredType.All:
                    bool oneNotValid = false;
                    foreach (string propertyName in PropertyList)
                    {
                        propertyInfo = value.GetType().GetProperty(propertyName);

                        if (propertyInfo != null)
                        {

                            var val = propertyInfo.GetValue(value, null);
                            ValidationResult result = base.IsValid(val, validationContext);

                            if (result != null)
                            {
                                oneNotValid = true;
                                break;
                            }
                        }
                    }

                    if (!oneNotValid)
                        return ValidationResult.Success;
                    break;

                case AllOrAtLeastOneRequiredType.AllIfOneIsNotNull:
                    bool oneNotValidInNotNull = false;
                    bool oneIsNotNull = false;
                    foreach (string propertyName in PropertyList)
                    {
                        propertyInfo = value.GetType().GetProperty(propertyName);

                        if (propertyInfo != null)
                        {

                            var val = propertyInfo.GetValue(value, null);
                            ValidationResult result = base.IsValid(val, validationContext);

                            if (result != null)
                                oneNotValidInNotNull = true;
                            else
                                oneIsNotNull = true;

                            if (oneNotValidInNotNull && oneIsNotNull)
                                break;
                        }
                    }

                    if (!oneNotValidInNotNull || !oneIsNotNull)
                        return ValidationResult.Success;
                    break;

                case AllOrAtLeastOneRequiredType.AtLeastOneRequired:

                    foreach (string propertyName in PropertyList)
                    {
                        propertyInfo = value.GetType().GetProperty(propertyName);

                        if (propertyInfo != null)
                        {

                            var val = propertyInfo.GetValue(value, null);
                            ValidationResult result = base.IsValid(val, validationContext);

                            if (result == null)
                                return ValidationResult.Success;
                        }
                    }
                    break;

            }


            var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

            string fields = string.Join<string>(", ", PropertyList);

            string errorMessage = localizer == null ? null : localizer[ErrorMessageString];

            if (string.IsNullOrWhiteSpace(errorMessage) || errorMessage == ErrorMessageString)
            {
                switch (AllOrAtLeastOneRequiredType)
                {
                    case AllOrAtLeastOneRequiredType.All:
                        errorMessage = DefaultValidationMessage.RequiredAll;
                        break;

                    case AllOrAtLeastOneRequiredType.AllIfOneIsNotNull:
                        errorMessage = DefaultValidationMessage.RequiredAllIfOneIsNotNull;
                        break;

                    case AllOrAtLeastOneRequiredType.AtLeastOneRequired:
                        errorMessage = DefaultValidationMessage.RequiredAtLeastOne;
                        break;
                }
            }

            errorMessage = string.Format(errorMessage, fields);

            return new ValidationResult(errorMessage);


        }
    }

}

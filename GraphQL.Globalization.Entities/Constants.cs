using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Entities
{
    public static class DefaultValidationMessage
    {
        public const string GreatherThan = "The field {0} must be greather than the field {1}.";
        public const string GreatherEqualsThan = "The field {0} must be greather or equals than the field {1}.";
        public const string RequiredAtLeastOne = "At least one between {0} is required.";
        public const string ValidEnum = "This field {0} has not valid enum value.";
        public const string RequiredAll = "All fields between {0} are required.";
        public const string RequiredAllIfOneIsNotNull = "All fields between {0} are required if at least one is not null.";

    }

    public static class Messages
    {
        public const string NoMoreThanOneSPInstance = "Can't set once a value has already been set.";
    }
}

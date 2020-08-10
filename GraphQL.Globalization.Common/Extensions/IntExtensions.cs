using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class IntExtensions
    {
        public static T ToEnum<T>(this int? value)
        {
            if (value == null)
                return default(T);

            return ToEnum<T>(value.Value);
        }

        public static T ToEnum<T>(this int value)
        {
            var result = default(T);

            try
            {

                var typeT = typeof(T);
                var underlyingType = Nullable.GetUnderlyingType(typeT);
                if (underlyingType != null)
                    typeT = underlyingType;

                if (Enum.IsDefined(typeT, value))
                {
                    result = (T)Enum.ToObject(typeT, value);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
    }
}

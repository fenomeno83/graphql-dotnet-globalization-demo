using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class StringExtensions
    {

        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            char[] a = s.Trim().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string FirstLetterToLowerCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            char[] a = s.Trim().ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }

        public static string NullableReplace(this string input, string source = ",", string destination = ".", bool keepEmptyOrNull = false, string defaultIfEmptyOrNull = null)
        {
            if (input == null)
                return !keepEmptyOrNull ? defaultIfEmptyOrNull : null;

            if (input.Trim() == string.Empty)
                return !keepEmptyOrNull ? defaultIfEmptyOrNull : input.Trim();

            return input.Replace(source, destination);
        }

        public static DateTime? DateTimeTryParseExact(this string StringDate, string format)
        {
            if (string.IsNullOrWhiteSpace(StringDate) || string.IsNullOrWhiteSpace(format))
                return null;

            DateTime castDate;
            if (DateTime.TryParseExact(StringDate.Trim(), format.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out castDate))
                return castDate;
            else
                return null;

        }

        public static string NullableTrim(this string input, bool keepEmptyOrNull = false, string defaultIfEmptyOrNull = null)
        {
            if (input == null)
                return !keepEmptyOrNull ? defaultIfEmptyOrNull : null;

            if (input.Trim() == string.Empty)
                return !keepEmptyOrNull ? defaultIfEmptyOrNull : input.Trim();

            return input.Trim();
        }

        public static string RemoveNewLineAndTrim(this string input)
        {
            if (input == null)
                return null;

            input = Regex.Replace(input, @"\t|\n|\r", "");
            input = input.Trim();

            return input;
        }

        public static string Left(this string param, int length)
        {
            var result = param.Substring(0, length);
            return result;
        }

        public static string Right(this string param, int length)
        {
            var result = param.Substring(param.Length - length, length);
            return result;
        }

        public static string Mid(this string param, int startIndex, int length)
        {
            var result = param.Substring(startIndex, length);
            return result;
        }

        public static DateTime? ToNullableDateTime(this string s)
        {
            var isDate = DateTime.TryParse(s, out var date);
            return (isDate) ? date : default(DateTime?);
        }

        public static IEnumerable<string> Split(this string str, int n)
        {
            if (string.IsNullOrEmpty(str) || n < 1)
            {
                throw new ArgumentException(Messages.NoStringInSubstrings);
            }

            for (var i = 0; i < str.Length; i += n)
            {
                yield return str.Substring(i, Math.Min(n, str.Length - i));
            }
        }

        public static string ReplaceTemplateFromDictionary(this string template, Dictionary<string, string> parameters)
        {
            return Regex.Replace(template, @"\{(.+?)\}", x => parameters[x.Groups[1].Value]);
        }

        public static T CustomCast<T>(this string input)
        {
            T output = default(T);
            if (string.IsNullOrWhiteSpace(input))
                return output;

            input = input.Trim();
            try
            {
                Type typeToCastTo = typeof(T);

                if (typeof(T).IsGenericType)
                    typeToCastTo = typeToCastTo.GenericTypeArguments[0];

                if (typeToCastTo.IsEnum)
                {
                    if (Enum.IsDefined(typeToCastTo, input))
                        return (T)Enum.Parse(typeToCastTo, input);
                    return output;
                }


                object value = Convert.ChangeType(input, typeToCastTo, CultureInfo.InvariantCulture);
                return (value == null) ? output : (T)value;
            }
            catch
            {
                return output;
            }
        }

        public static JToken GetBodyObject(this string body)
        {
            return string.IsNullOrWhiteSpace(body) ? null : JToken.Parse(body);
        }

        public static T GetBodyObject<T>(this string body)
        {
            return string.IsNullOrWhiteSpace(body) ? default(T) : JsonConvert.DeserializeObject<T>(body);
        }
    }
}

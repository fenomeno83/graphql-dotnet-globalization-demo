using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class JsonExtensions
    {
        public static string GetFlattenValuesFromJObject(this JObject jobject)
        {
            var sb = new StringBuilder();
            var jTokens = jobject.Descendants().Where(p => p.Count() == 0);

            var results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) =>
            {
                properties.Add(jToken.Path, jToken.ToString());
                return properties;
            });

            foreach (var (key, value) in results)
            {
                sb.Append($"{key} - {value}");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public static bool HasProperty(this JToken jtoken, string property)
        {
            var obj = jtoken?.SelectToken(property);

            return obj == null ? false : true;
        }

        public static bool HasProperty(this JObject jobject, string property)
        {
            var obj = jobject?.SelectToken(property);

            return obj == null ? false : true;
        }
    }

}

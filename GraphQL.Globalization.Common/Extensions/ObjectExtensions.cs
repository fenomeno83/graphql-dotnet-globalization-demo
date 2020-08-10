using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
                return null;

            return (Dictionary<string, object>)obj;

        }

        public static string NullableToString(this object obj, string defaultIfNull = null)
        {
            if (obj == null)
                return defaultIfNull;

            return obj.ToString();
        }

        public static T CloneObject<T>(this object objSource)
        {
            if (objSource == null)
                return default(T);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, objSource);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T CastObject<T>(this object objSource)
        {
            if (objSource == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objSource));
        }

        public static List<string> PropertiesFromType(this object atype)
        {
            if (atype == null) return new List<string>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> propNames = new List<string>();
            foreach (PropertyInfo prp in props)
            {
                propNames.Add(prp.Name);
            }
            return propNames;
        }
    }
}

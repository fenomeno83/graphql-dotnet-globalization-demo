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

        public static T CloneObject<T>(this T objSource, bool isSerializable = false)
        {
            if (objSource == null)
                return default(T);

            if (isSerializable)
            {
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, objSource);
                    stream.Position = 0;
                    return (T)formatter.Deserialize(stream);
                }
            }
            else
                return objSource.CastObject<T>();
        }

        public static T TryCloneObject<T>(this T objSource, bool isSerializable = false)
        {
            if (objSource == null)
                return default(T);

            if (isSerializable)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(stream, objSource);
                        stream.Position = 0;
                        return (T)formatter.Deserialize(stream);
                    }
                }
                catch
                {
                    return objSource.TryCastObject<T>();
                }

            }
            else
                return objSource.TryCastObject<T>();
        }

        public static T CastObject<T>(this object objSource)
        {
            if (objSource == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(objSource));
        }

        public static T TryCastObject<T>(this object objSource)
        {
            if (objSource == null)
                return default(T);

            return JsonConvert.SerializeObject(objSource).TryDeserializeObject<T>();
        }

        public static List<string> PropertiesFromType(this object atype, string pre = null)
        {
            if (atype == null) return new List<string>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            List<string> propNames = new List<string>();
            foreach (PropertyInfo prp in props)
            {
                var propType = prp.PropertyType;
                if (propType != null && propType != typeof(string) && propType.IsClass)
                {
                    var source = prp.GetValue(atype, null);
                    if (source != null)
                    {
                        List<string> typeList = PropertiesFromType(source, $"{(pre ?? string.Empty)}{prp.Name}.");
                        if (typeList.Count > 0)
                            propNames.AddRange(typeList);
                    }
                }
                else
                    propNames.Add($"{pre ?? string.Empty}{prp.Name}");
            }
            return propNames;
        }
    }
}

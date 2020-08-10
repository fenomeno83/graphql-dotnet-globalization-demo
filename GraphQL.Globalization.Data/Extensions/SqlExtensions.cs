using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;
using System;

namespace GraphQL.Globalization.Data.Extensions
{
    //use toSql method only for debug purpose, to generate raw sql query from entity framework.
    //as alternative you can enable "Microsoft.EntityFrameworkCore": "Debug" in appsettings, but it don't generate query with replaced parameters
    public static class SqlExtensions
    {
        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);

        /// <summary>
        /// Gets a SQL statement from an IQueryable
        /// </summary>
        /// <param name="query">The query to get the SQL statement for</param>
        /// <returns>Formatted SQL statement as a string</returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            using var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");
            var relationalQueryContext = enumerator.Private<RelationalQueryContext>("_relationalQueryContext");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);
            var parametersDict = relationalQueryContext.ParameterValues;

            return SubstituteVariables(command.CommandText, parametersDict);
        }

        private static string SubstituteVariables(string commandText, IReadOnlyDictionary<string, object> parametersDictionary)
        {
            var sql = commandText;
            foreach (var (key, value) in parametersDictionary)
            {
                var placeHolder = "@" + key;
                var actualValue = GetActualValue(value);
                sql = sql.Replace(placeHolder, actualValue);
            }

            return sql;
        }

        private static string GetActualValue(object value)
        {
            var type = value.GetType();

            if (type.IsNumeric())
                return value.ToString();

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                switch (type.Name)
                {
                    case nameof(DateTime):
                        return $"'{(DateTime)value:u}'";

                    case nameof(DateTimeOffset):
                        return $"'{(DateTimeOffset)value:u}'";
                }
            }

            return $"'{value}'";
        }

        private static bool IsNullable(this Type type)
        {
            return
                type != null &&
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsNumeric(this Type type)
        {
            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type);

            if (type == null || type.IsEnum)
                return false;

            return Type.GetTypeCode(type) switch
            {
                TypeCode.Byte => true,
                TypeCode.Decimal => true,
                TypeCode.Double => true,
                TypeCode.Int16 => true,
                TypeCode.Int32 => true,
                TypeCode.Int64 => true,
                TypeCode.SByte => true,
                TypeCode.Single => true,
                TypeCode.UInt16 => true,
                TypeCode.UInt32 => true,
                TypeCode.UInt64 => true,
                _ => false
            };
        }
    }
}

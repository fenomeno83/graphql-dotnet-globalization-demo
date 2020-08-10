﻿using System;
using System.Linq;

namespace GraphQL.Globalization.Entities.Extensions
{
    public static class EnumExtension
    {
        public class OrderAttribute : Attribute
        {
            public readonly int Order;

            public OrderAttribute(int order)
            {
                Order = order;
            }
        }

        public static T[] SortEnum<T>()
        {
            Type myEnumType = typeof(T);
            var enumValues = Enum.GetValues(myEnumType).Cast<T>().ToArray();
            var enumNames = Enum.GetNames(myEnumType);
            int[] enumPositions = Array.ConvertAll(enumNames, n =>
            {
                OrderAttribute orderAttr = (OrderAttribute)myEnumType.GetField(n)
                    .GetCustomAttributes(typeof(OrderAttribute), false)[0];
                return orderAttr.Order;
            });

            Array.Sort(enumPositions, enumValues);

            return enumValues;
        }

    }
}

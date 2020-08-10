using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Common.Extensions
{
    public static class ListExtensions
    {
        public static void Move<T>(this List<T> list, Predicate<T> itemSelector, int newIndex)
        {
            if (list == null || list.Count == 0)
                return;

            if (newIndex < 0 || newIndex > list.Count - 1)
                return;

            var currentIndex = list.FindIndex(itemSelector);
            if (currentIndex < 0)
                return;

            // Copy the current item
            var item = list[currentIndex];

            // Remove the item
            list.RemoveAt(currentIndex);

            // Finally add the item at the new index
            list.Insert(newIndex, item);
        }

        public static List<T> AddAndReturn<T>(this List<T> list, T elem)
        {
            if (list == null)
                return default(List<T>);

            list.Add(elem);

            return list;
        }
    }
}

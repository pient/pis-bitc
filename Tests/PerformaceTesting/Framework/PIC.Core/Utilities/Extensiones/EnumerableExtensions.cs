﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create an array from an IList.
        /// </summary>
        /// <param name="targetType">Type of the item in the array.</param>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static Array BuildArray<T>(this ICollection list)
        {
            Array array = Array.CreateInstance(typeof(T), list.Count);

            list.CopyTo(array, 0);

            return array;
        }

        public static IList<T> BuildList<T>(this ICollection list)
        {
            IList<T> lists = list.Cast<T>().ToList();

            return lists;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> addedList)
        {
            foreach (T it in addedList)
            {
                if (!list.Contains(it))
                {
                    list.Add(it);
                }
            }
        }
    }
}

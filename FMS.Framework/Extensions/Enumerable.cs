using System;
using System.Collections.Generic;

namespace FMS.Framework.Extensions
{
    public static class Enumerable
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }
    }
}

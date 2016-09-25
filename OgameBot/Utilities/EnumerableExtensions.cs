using System;
using System.Collections.Generic;
using System.Linq;

namespace OgameBot.Utilities
{
    public static class EnumerableExtensions
    {
        public static HashSet<T> ToHashset<T>(this IEnumerable<T> set)
        {
            return new HashSet<T>(set);
        }

        public static HashSet<TKey> ToHashset<TInput, TKey>(this IEnumerable<TInput> set, Func<TInput, TKey> selector)
        {
            return new HashSet<TKey>(set.Select(selector));
        }
    }
}
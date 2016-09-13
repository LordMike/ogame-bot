using System;
using System.Collections.Generic;

namespace OgameBot.Utilities
{
    public static class DictionaryExtensions
    {
        public static TVal AddOrUpdate<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key, Func<TVal> addFunc, Func<TKey, TVal, TVal> updateFunc)
        {
            TVal val;
            if (dict.TryGetValue(key, out val))
                return dict[key] = updateFunc(key, val);
            
            return dict[key] = addFunc();
        }

        public static TVal GetOrAdd<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key, Func<TVal> addFunc)
        {
            TVal val;
            if (dict.TryGetValue(key, out val))
                return val;

            val = addFunc();
            dict.Add(key, val);
            return val;
        }
    }
}
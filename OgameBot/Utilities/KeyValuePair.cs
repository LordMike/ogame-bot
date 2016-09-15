using System.Collections.Generic;

namespace OgameBot.Utilities
{
    public static class KeyValuePair
    {
        public static KeyValuePair<TKey, TVal> Create<TKey, TVal>(TKey key, TVal val)
        {
            return new KeyValuePair<TKey, TVal>(key, val);
        }
    }
}
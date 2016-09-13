using System.Collections.Generic;
using System.Collections.Specialized;

namespace ScraperClientLib.Utilities
{
    public static class NameValueCollectionExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> AsKeyValues(this NameValueCollection nvc)
        {
            foreach (string key in nvc)
            {
                yield return new KeyValuePair<string, string>(key, nvc[key]);
            }
        }
    }
}
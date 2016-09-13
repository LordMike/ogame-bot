using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace OgameBot.Utilities
{
    public static class HtmlNodeExtensions
    {
        public static IEnumerable<string> GetCssClasses(this HtmlNode node)
        {
            return node.GetAttributeValue("class", string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> GetCssClasses(this HtmlNode node, Regex predicate)
        {
            return node.GetAttributeValue("class", string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(s => predicate.IsMatch(s));
        }

        public static IEnumerable<string> GetCssClasses(this HtmlNode node, Func<string, bool> predicate)
        {
            return node.GetAttributeValue("class", string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Where(predicate);
        }
    }
}

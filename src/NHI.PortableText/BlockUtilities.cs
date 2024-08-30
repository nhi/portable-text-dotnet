using HtmlAgilityPack;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NHI.PortableText
{
    /// <summary>
    /// Some usefull utilities when parsing html into block models
    /// </summary>
    public static class BlockUtilities
    {
        /// <summary>
        /// Gets the list style for a node by looking backwards into the tree for the first list node
        /// </summary>
        public static string GetListStyle(HtmlNode node)
        {
            if (node == null) return "bullet";

            switch(node.Name)
            {
                case "ul": return "bullet";
                case "ol": return "number";
                default: return GetListStyle(node.ParentNode);
            };
        }

        /// <summary>
        /// Gets the level of a node by looking backwards into the tree and counting identation-elements
        /// </summary>
        public static int GetListLevel(HtmlNode node)
        {
            if (node == null) return 0;

            return GetListLevel(node.ParentNode) + (new[] { "ul", "ol", "dl" }.Contains(node.Name) ? 1 : 0);     
        }

        /// <summary>
        /// Converts html-encodings to regular strings and normalizes whitespace
        /// </summary>
        public static string HtmlDecode(string html)
        {
            return HttpUtility.HtmlDecode(Regex.Replace(html, @"[^\S\xa0]+", " ")); //Changed to not touch non-breaking spaces
        }

        /// <summary>
        /// Generate a "unique" key to use for markDefs
        /// </summary>
        public static string GenerateKey() => Math.Abs(Guid.NewGuid().ToString().GetHashCode()).ToString("X");
    }

}

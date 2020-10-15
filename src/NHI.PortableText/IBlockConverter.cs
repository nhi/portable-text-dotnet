using HtmlAgilityPack;
using NHI.PortableText.Model;
using NHI.PortableText.Parser;
using System;
using System.Collections.Generic;

namespace NHI.PortableText
{
    /// <summary>
    /// Interface for BlockConverter
    /// </summary>
    public interface IBlockConverter
    {
        /// <summary>
        /// A list of all registered parsers to be manually modified if one should so desire
        /// </summary>
        List<IHtmlNodeParser> Parsers { get; set; }

        /// <summary>
        /// Add a parser to the converter
        /// </summary>
        void AddParser(IHtmlNodeParser instance);

        /// <summary>
        /// Remove a parser from the converter
        /// </summary>
        void RemoveParser(Type type);

        /// <summary>
        /// Take a Html string and return it as serialized block data
        /// </summary>
        string SerializeHtml(string html);

        /// <summary>
        /// Convert a html string to a List of BlockModel
        /// </summary>
        List<BlockModel> ConvertHtml(string html);

        /// <summary>
        /// Parse a HtmlNode
        /// </summary>
        /// <param name="root">The list where root level blocks should be added</param>
        /// <param name="parent">The block where spans should be added</param>
        /// <param name="node">The node to be parsed</param>
        void ParseNode(List<BlockModel> root, BlockModel parent, HtmlNode node);
    }
}
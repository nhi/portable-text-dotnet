using HtmlAgilityPack;
using NHI.PortableText.Model;
using System.Collections.Generic;

namespace NHI.PortableText.Parser
{
    /// <summary>
    /// The interface which must be implemented by all parsers registered with the converter
    /// </summary>
    public interface IHtmlNodeParser
    {
        /// <summary>
        /// Should evaluate the node and return true if you want to handle it
        /// </summary>
        bool IsParserFor(HtmlNode node);

        /// <summary>
        /// Parse an HtmlNode into block data
        /// </summary>
        /// <param name="converter">The converter to use when parsing child nodes</param>
        /// <param name="root">The root where all block models should be added</param>
        /// <param name="parent">The block where all spans should be added</param>
        /// <param name="node">The node to read from</param>
        void Parse(IBlockConverter converter, List<BlockModel> root, BlockModel parent, HtmlNode node);
    }
}

using HtmlAgilityPack;
using NHI.PortableText.Model;
using System.Collections.Generic;
using System.Linq;

namespace NHI.PortableText.Parser
{
    /// <summary>
    /// The default block parser. Will parse all child nodes and attach the created blockdata to the root.
    /// If there are nested blocks or stray spans, the block will be broken into multiples and the nested elements
    /// are inserted into the root in the proper order.
    /// </summary>
    public abstract class BaseBlockParser : IHtmlNodeParser
    {
        protected virtual BlockModel CreateBlock(HtmlNode node) => new BlockModel();

        public virtual bool IsParserFor(HtmlNode node) => node.Name == "";

        public virtual void Parse(IBlockConverter converter, List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            var tempRoot = new List<BlockModel>();
            
            var block = CreateBlock(node);                      

            foreach (var child in node.ChildNodes)
            {
                converter.ParseNode(tempRoot, block, child);

                if (tempRoot.Any())                             
                {
                    if (block.Children.Any()) root.Add(block);

                    foreach (var innerBlock in tempRoot)
                    {
                        root.Add(innerBlock);
                    }

                    tempRoot = new List<BlockModel>();
                    block = CreateBlock(node);
                }
            }

            if (block.Children.Any()) root.Add(block);

            foreach (var innerBlock in tempRoot)
            {
                root.Add(innerBlock);
            }

        }
    }

    /// <summary>
    /// Parser for the <blockquote></blockquote> tag
    /// </summary>
    public class BlockQuoteParser : BaseBlockParser
    {
        public override bool IsParserFor(HtmlNode n) => n.Name == "blockquote";
        protected override BlockModel CreateBlock(HtmlNode node) => new BlockModel(style: "blockquote");
    }

    /// <summary>
    /// Parser for the <div></div> tag
    /// </summary>
    public class DivNodeParser : BaseBlockParser
    {
        public override bool IsParserFor(HtmlNode n) => n.Name == "div";
    }

    /// <summary>
    /// Parser for the <td></td> tag
    /// </summary>
    public class TdNodeParser : BaseBlockParser
    {
        public override bool IsParserFor(HtmlNode n) => n.Name == "td";
    }

    /// <summary>
    /// Parser for the <p></p> tag
    /// </summary>
    public class ParagraphNodeParser : BaseBlockParser
    {
        public override bool IsParserFor(HtmlNode n) => n.Name == "p";
    }

    /// <summary>
    /// Parser for the <li></li> tag"
    /// </summary>
    public class LiNodeParser : BaseBlockParser
    {
        protected override BlockModel CreateBlock(HtmlNode node) =>
            new BlockModel(listItem: BlockUtilities.GetListStyle(node), level: BlockUtilities.GetListLevel(node));

        public override bool IsParserFor(HtmlNode n) => n.Name == "li";
    }

    /// <summary>
    /// Parser for the <dt></dt> tag
    /// </summary>
    public class DtNodeParser : BaseBlockParser
    {
        protected override BlockModel CreateBlock(HtmlNode node) =>
            new BlockModel(listItem: BlockUtilities.GetListStyle(node), level: BlockUtilities.GetListLevel(node));

        public override bool IsParserFor(HtmlNode n) => n.Name == "dt";
    }

    /// <summary>
    /// Parser for the <dd></dd> tag
    /// </summary>
    public class DdNodeParser : BaseBlockParser
    {
        protected override BlockModel CreateBlock(HtmlNode node) =>
            new BlockModel(listItem: BlockUtilities.GetListStyle(node), level: BlockUtilities.GetListLevel(node));

        public override bool IsParserFor(HtmlNode n) => n.Name == "dd";
    }

    /// <summary>
    /// Parser for the <h1></h1> through <h6></h6> tags
    /// </summary>
    public class HeadingNodeParser : BaseBlockParser
    {
        protected override BlockModel CreateBlock(HtmlNode node) => new BlockModel(style: node.Name);

        public override bool IsParserFor(HtmlNode n) => new[] { "h1", "h2", "h3", "h4", "h5", "h6" }.Contains(n.Name);
    }

}

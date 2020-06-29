using HtmlAgilityPack;
using NHI.PortableText.Model;
using System.Collections.Generic;

namespace NHI.PortableText.Parser
{
    /// <summary>
    /// The base span parser. Adds a span model to the parent block and parses children.
    /// </summary>
    public abstract class BaseSpanParser : IHtmlNodeParser
    {
        protected virtual SpanModel CreateSpan(HtmlNode node) => new SpanModel(text: node.InnerText);

        public virtual bool IsParserFor(HtmlNode n) => n.Name == "";

        public virtual void Parse(IBlockConverter converter, List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            var span = CreateSpan(node);

            if (span != null) parent.Children.Add(span);

            foreach (var child in node.ChildNodes)
                converter.ParseNode(root, parent, child);
        }
    }

    /// <summary>
    /// Parser for all text nodes. Tries to be clever about whitespace, might fail.
    /// </summary>
    public class TextNodeParser : BaseSpanParser
    {
        protected override SpanModel CreateSpan(HtmlNode node)
        {
            if (string.IsNullOrWhiteSpace(node.InnerText)) return null;
            return new SpanModel(text: BlockUtilities.HtmlDecode(node.InnerText));
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "#text";
    }

    /// <summary>
    /// Parser for the <br></br> tag
    /// </summary>
    public class BrNodeParser : BaseSpanParser
    {
        protected override SpanModel CreateSpan(HtmlNode node) => new SpanModel(text: "\n");

        public override bool IsParserFor(HtmlNode n) => n.Name == "br";
    }
}

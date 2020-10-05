using HtmlAgilityPack;
using NHI.PortableText.Model;
using System.Collections.Generic;
using System.Linq;

namespace NHI.PortableText.Parser
{
    /// <summary>
    /// The default decorator parser. Sets the markDef for spans on the block and makes sure all
    /// child spans that are created gets the markDefs as well.
    /// </summary>
    public abstract class BaseDecoratorParser : IHtmlNodeParser
    {
        protected virtual string GetMark(HtmlNode node) => "";

        protected virtual MarkDefModel CreateMarkDef(HtmlNode node) => null;

        public virtual bool IsParserFor(HtmlNode n) => n.Name == "";

        public virtual void Parse(IBlockConverter builder, List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            var block = new BlockModel();

            foreach (var child in node.ChildNodes)
            {
                builder.ParseNode(root, block, child);
            }

            foreach (var child in block.Children)
            {
                child.Marks.Add(GetMark(node));
                parent.Children.Add(child);
            }

            parent.MarkDefs.AddRange(block.MarkDefs);

            var markDef = CreateMarkDef(node);
            if (markDef != null) parent.MarkDefs.Add(markDef);
        }
    }

    /// <summary>
    /// Parser for the <a></a> tag
    /// </summary>
    public class AnchorNodeParser : BaseDecoratorParser
    {
        private string _key = null;

        protected override MarkDefModel CreateMarkDef(HtmlNode node) =>
            new MarkDefModel
            {
                Key = GetMark(node),
                Type = "link",
                ["href"] = node.GetAttributeValue<string>("href", string.Empty)
            };

        protected override string GetMark(HtmlNode node) => _key ?? (_key = BlockUtilities.GenerateKey());

        public override bool IsParserFor(HtmlNode n) => n.Name == "a";

        public override void Parse(IBlockConverter builder, List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            base.Parse(builder, root, parent, node);
            _key = null;
        }
    }

    /// <summary>
    /// Parser for the <sup></sup> tag
    /// </summary>
    public class SupNodeParser : BaseDecoratorParser
    {
        public override bool IsParserFor(HtmlNode n) => n.Name == "sup";
        protected override string GetMark(HtmlNode n) => "superscript";
    }

    /// <summary>
    /// Parser for the <sub></sub> tag
    /// </summary>
    public class SubNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "subscript";
        public override bool IsParserFor(HtmlNode n) => n.Name == "sub";
    }

    /// <summary>
    /// Parser for the <strong></strong> and <b></b> tags
    /// </summary>
    public class StrongNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "strong";
        public override bool IsParserFor(HtmlNode n) => new[] { "b", "strong" }.Contains(n.Name);
    }

    /// <summary>
    /// Parser for the <em></em> tag
    /// </summary>
    public class EmNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "em";
        public override bool IsParserFor(HtmlNode n) => n.Name == "em";
    }

    /// <summary>
    /// Parser for the <i></i> tag 
    /// </summary>
    public class ItalicNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "em";
        public override bool IsParserFor(HtmlNode n) => n.Name == "i";
    }

    /// <summary>
    /// Parser for the <u></u> tag
    /// </summary>
    public class UNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "underline";
        public override bool IsParserFor(HtmlNode n) => n.Name == "u";
    }

    /// <summary>
    /// Parser for the <s></s> and <strike></strike> tags
    /// </summary>
    public class StrikeNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "strike-through";
        public override bool IsParserFor(HtmlNode n) => new[] { "s", "strike" }.Contains(n.Name);
    }

    /// <summary>
    /// Parser for the <del></del> tag
    /// </summary>
    public class DelNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "strike-through";
        public override bool IsParserFor(HtmlNode n) => n.Name == "del";
    }

    /// <summary>
    /// Parser for the <code></code> tag
    /// </summary>
    public class CodeNodeParser : BaseDecoratorParser
    {
        protected override string GetMark(HtmlNode n) => "code";
        public override bool IsParserFor(HtmlNode n) => n.Name == "code";
    }

}


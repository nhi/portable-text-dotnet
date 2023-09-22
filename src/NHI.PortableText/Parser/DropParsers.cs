using HtmlAgilityPack;
using NHI.PortableText.Model;
using System.Collections.Generic;

namespace NHI.PortableText.Parser
{
    /// <summary>
    /// Default drop parser. This parser does nothing and makes sure no children are parsed.
    /// </summary>
    public abstract class BaseDropParser : IHtmlNodeParser
    {
        public virtual bool IsParserFor(HtmlNode node) => node.Name == "";

        public virtual void Parse(IBlockConverter converter, List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            //do nothing with the dropped node, or any children
        }
    }

    /// <summary>
    /// Parser for the <head></head> tag
    /// </summary>
    public class HeadNodeParser : BaseDropParser
    {
        public HeadNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "head";
    }

    /// <summary>
    /// Parser for html comments
    /// </summary>
    public class CommentNodeParser : BaseDropParser
    {
        public CommentNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "#comment";
    }

    /// <summary>
    /// Parser for the <hr></hr> tag
    /// </summary>
    public class HrNodeParser : BaseDropParser
    {
        public HrNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "hr";
    }

    /// <summary>
    /// Parser for the <svg></svg> tag
    /// </summary>
    public class SvgNodeParser : BaseDropParser
    {
        public SvgNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "svg";
    }

    /// <summary>
    /// Parser for the <img></img> tag
    /// </summary>
    public class ImgNodeParser : BaseDropParser
    {
        public ImgNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "img";
    }

    /// <summary>
    /// Parser for the <input></input> tag
    /// </summary>
    public class InputNodeParser : BaseDropParser
    {
        public InputNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "input";
    }
}

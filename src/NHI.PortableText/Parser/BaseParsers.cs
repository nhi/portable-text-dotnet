﻿using HtmlAgilityPack;
using NHI.PortableText.Model;
using System.Collections.Generic;

namespace NHI.PortableText.Parser
{
    /// <summary>
    /// The default parser. Does no operation other than making sure all child nodes are parsed
    /// </summary>
    public abstract class BaseParser : IHtmlNodeParser
    {
        public virtual bool IsParserFor(HtmlNode node) => node.Name == "";

        public virtual void Parse(IBlockConverter converter, List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            foreach (var child in node.ChildNodes)
            {
                converter.ParseNode(root, parent, child);
            }
        }
    }

    /// <summary>
    /// Parser for the '#document' HtmlNode. 
    /// </summary>
    public class RootNodeParser : BaseParser
    {
        public RootNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "#document";
    }

    /// <summary>
    /// Parser for the <html></html>html> tag
    /// </summary>
    public class HtmlNodeParser : BaseParser
    {
        public HtmlNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "html";
    }

    /// <summary>
    /// Parser for the <body></body> tag
    /// </summary>
    public class BodyNodeParser : BaseParser
    {
        public BodyNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "body";
    }

    /// <summary>
    /// Parser for the <pre></pre> tag
    /// </summary>
    public class PreNodeParser : BaseParser
    {
        public PreNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode node) => node.Name == "pre";
    }

    /// <summary>
    /// Parser for the <table></table> tag
    /// </summary>
    public class TableNodeParser : BaseParser
    {
        public TableNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode node) => node.Name == "table";
    }

    /// <summary>
    /// Parser for the <tbody></tbody> tag
    /// </summary>
    public class TBodyNodeParser : BaseParser
    {
        public TBodyNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "tbody";
    }

    /// <summary>
    /// Parser for the <tr></tr> tag
    /// </summary>
    public class TrNodeParser : BaseParser
    {
        public TrNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "tr";
    }

    /// <summary>
    /// Parser for the <ul></ul> tag
    /// </summary>
    public class UlNodeParser : BaseParser
    {
        public UlNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "ul";
    }

    /// <summary>
    /// Parser for the <ol></ol> tag
    /// </summary>
    public class OlNodeParser : BaseParser
    {
        public OlNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "ol";
    }

    /// <summary>
    /// Parser for the <dl></dl> tag
    /// </summary>
    public class DlNodeParser : BaseParser
    {
        public DlNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "dl";
    }

    /// <summary>
    /// Parser for the <span></span> tag
    /// </summary>
    public class SpanNodeParser : BaseParser
    {
        public SpanNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "span";
    }

    /// <summary>
    /// Parser for the <label></label> tag
    /// </summary>
    public class LabelNodeParser : BaseParser
    {
        public LabelNodeParser(object _ = null)
        {
        }

        public override bool IsParserFor(HtmlNode n) => n.Name == "label";
    }
}

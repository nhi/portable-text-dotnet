using System;
using Xunit;

namespace NHI.PortableText.Test
{
    /// <summary>
    /// Tests of BlockConverter configuration and tag-recognition
    /// </summary>
    public class BlockConverter
    {
        [Fact]
        public void Should_throw_exception_on_unknown_tag()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.ThrowException,
            };

            var converter = new PortableText.BlockConverter(settings);
            Assert.ThrowsAny<Exception>(() => converter.SerializeHtml("<unknownTag>a</unknownTag>"));
        }

        [Fact]
        public void Should_throw_exception_on_unnested_span()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.ThrowException,
            };

            var converter = new PortableText.BlockConverter(settings);
            Assert.ThrowsAny<Exception>(() => converter.SerializeHtml("<b>a<b>"));
        }

        [Fact]
        public void Should_not_throw_exception_on_unknown_tag()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.HandleChildren,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            var converter = new PortableText.BlockConverter(settings);
            converter.SerializeHtml("<unknownTag>a</unknownTag>");
            Assert.True(true);
        }

        [Fact]
        public void Should_not_throw_exception_on_unnested_span()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.HandleChildren,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            var converter = new PortableText.BlockConverter(settings);
            converter.SerializeHtml("<b>a</b>");
            Assert.True(true);
        }

        [Fact]
        public void Should_recognize_implemented_tags()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            var converter = new PortableText.BlockConverter(settings);
            var implementedTags = new[] 
            {
                "#document", "html", "body", "pre", "table", "tbody", "tr", "ul", "ol", "dl", "span", "label",
                "blockquote", "div", "td", "p", "li", "dt", "dd", "h1", "h2", "h3", "h4", "h5", "h6",
                "sup", "sub", "strong", "b", "em", "i", "u", "s", "strike", "del", "code",
                "head", "#comment", "hr", "svg", "img", "input","#text", "br"
            };

            foreach(var tag in implementedTags)
                converter.SerializeHtml($"<{tag}>a</{tag}>");

            Assert.True(true);
        }

        [Fact]
        public void Should_generate_unique_keys()
        {
            var a = BlockUtilities.GenerateKey();
            var b = BlockUtilities.GenerateKey();
            Assert.NotEqual(a, b);
        }
    }
}

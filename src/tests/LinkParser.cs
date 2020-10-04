using Xunit;

namespace PortableText.Test
{
    /// <summary>
    /// Tests of link parsing
    /// </summary>
    public class LinkParser
    {
        private IBlockConverter _converter;

        public LinkParser()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_set_correct_mark_on_block()
        {
            var bc = _converter.ConvertHtml("<p>This is a <a href=\"http\">link</a></p>");
            Assert.Equal(bc[0].MarkDefs[0].Key, bc[0].Children[1].Marks[0]);
        }

        [Fact]
        public void Should_set_markdef_properties()
        {
            var bc = _converter.ConvertHtml("<p>This is a <a href=\"http\">link</a></p>");
            Assert.Equal("link", bc[0].MarkDefs[0].Type);
            Assert.Equal("http", bc[0].MarkDefs[0]["href"]);
        }

        [Fact]
        public void Should_preseve_inner_decorations()
        {
            var bc = _converter.ConvertHtml("<p>This is a <a href=\"http\"><b>link</b></a></p>");
            Assert.Equal(bc[0].MarkDefs[0].Key, bc[0].Children[1].Marks[1]);
            Assert.Equal("strong", bc[0].Children[1].Marks[0]);
        }

        [Fact]
        public void Should_preseve_outer_decorations()
        {
            var bc = _converter.ConvertHtml("<p><b>This is a <a href=\"http\">link</a></b></p>");
            Assert.Equal(bc[0].MarkDefs[0].Key, bc[0].Children[1].Marks[0]);
            Assert.Equal("strong", bc[0].Children[1].Marks[1]);
        }

        [Fact]
        public void Should_apply_markdefs_to_multiple_spans()
        {
            var bc = _converter.ConvertHtml("<p>This is a <a href=\"http\">link with multiple <b>spans</b></a></p>");
            Assert.Equal(bc[0].MarkDefs[0].Key, bc[0].Children[1].Marks[0]);
            Assert.Equal(bc[0].MarkDefs[0].Key, bc[0].Children[2].Marks[1]);
        }

        [Fact]
        public void Should_handle_multiple_links_in_a_single_block()
        {
            var bc = _converter.ConvertHtml("<p>This is a <a href=\"http1\">link</a> and <a href=\"http2\">another</a></p>");
            Assert.NotEqual(bc[0].MarkDefs[0].Key, bc[0].MarkDefs[1].Key);
            Assert.Equal(bc[0].MarkDefs[0].Key, bc[0].Children[1].Marks[0]);
            Assert.Equal(bc[0].MarkDefs[1].Key, bc[0].Children[3].Marks[0]);
        }
    }
}

using Xunit;

namespace PortableText.Test
{
    /// <summary>
    /// Tests of abstract BaseSpanParser via implementations
    /// </summary>
    public class BaseSpanParser
    {
        private IBlockConverter _converter;

        public BaseSpanParser()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_create_children()
        {
            var bc = _converter.ConvertHtml("<br/>");
            Assert.Equal("\n", bc[0].Children[0].Text);
        }

        [Fact]
        public void Should_handle_stray_text()
        {
            var bc = _converter.ConvertHtml("text without markup");
            Assert.Equal("text without markup", bc[0].Children[0].Text);
        }
    }
}

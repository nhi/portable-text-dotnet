using Xunit;

namespace PortableText.Test
{
    /// <summary>
    /// Tests of abstract BaseParser via implementations
    /// </summary>
    public class BaseParser
    {
        private IBlockConverter _converter;

        public BaseParser()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_handle_child_nodes()
        {
            var pt = _converter.SerializeHtml($"<html>This should be parsed</html>");
            Assert.Contains("This should be parsed", pt);
        }

    }
}

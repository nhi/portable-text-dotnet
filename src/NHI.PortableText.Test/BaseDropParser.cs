using Xunit;

namespace NHI.PortableText.Test
{
    /// <summary>
    /// Tests of abstract BaseDropParser via implementations
    /// </summary>
    public class BaseDropParser
    {
        private IBlockConverter _converter;

        public BaseDropParser()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_not_parse_children()
        {
            var pt = _converter.SerializeHtml("<head><p>Should not be parsed</p></head>");
            Assert.DoesNotContain("Should not be parsed", pt);
        }

    }
}

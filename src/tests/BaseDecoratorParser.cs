using Xunit;

namespace PortableText.Test
{
    /// <summary>
    /// Tests of abstract BaseDecoratorParser via implementations
    /// </summary>
    public class BaseDecoratorParser
    {
        private IBlockConverter _converter;

        public BaseDecoratorParser()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_set_mark()
        {
            var bc = _converter.ConvertHtml("<div><b>this is bold</b></div>");
            Assert.Contains("strong", bc[0].Children[0].Marks[0]);
        }

        [Fact]
        public void Should_set_nested_marks()
        {
            var bc = _converter.ConvertHtml("<div><b>this is bold<u>underlined</u>bold again</b></div>");
            Assert.Contains("strong", bc[0].Children[0].Marks[0]);
            Assert.Contains("underline", bc[0].Children[1].Marks[0]);
            Assert.Contains("strong", bc[0].Children[1].Marks[1]);
            Assert.Contains("strong", bc[0].Children[2].Marks[0]);
        }
    }
}

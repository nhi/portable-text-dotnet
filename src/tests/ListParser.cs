using System.Linq;
using Xunit;

namespace PortableText.Test
{
    /// <summary>
    /// Tests of parsing of ul, ol and li tags
    /// </summary>
    public class ListParser
    {
        private IBlockConverter _converter;

        public ListParser()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_handle_correct_nesting()
        {
            var bc = _converter.ConvertHtml("<ul><li><ul><li>double indent</li></ul></li></ul>");
            Assert.Equal(2, bc[0].Level);
        }

        [Fact]
        public void Should_handle_incorrect_nesting()
        {
            var bc = _converter.ConvertHtml("<ul><ul><li>invalid nesting</li></ul></ul>");
            Assert.Equal(2, bc[0].Level);
        }

        [Fact]
        public void Should_set_correct_list_style()
        {
            var bc = _converter.ConvertHtml("<ul><li>entry</li></ul><ol><li>entry</li></ol>");
            Assert.Equal("bullet", bc[0].ListItem);
            Assert.Equal("number", bc[1].ListItem);
        }

        [Fact]
        public void Should_preserve_order_of_list_elements()
        {
            var bc = _converter.ConvertHtml("<ul><li>1</li><li>2</li><ul><li>3</li></ul><li>4</li></ul>");
            var nums = bc.SelectMany(p => p.Children.Select(p => p.Text));
            Assert.Equal("1234", string.Join("", nums));
        }

        [Fact]
        public void Should_preserve_order_with_nested_blocks()
        {
            var bc = _converter.ConvertHtml("<ul><li>1</li><li>2</li><ul><div>3</div><li>4</li></ul><li>5</li></ul>");
            var nums = bc.SelectMany(p => p.Children.Select(p => p.Text));
            Assert.Equal("12345", string.Join("", nums));
        }
    }
}

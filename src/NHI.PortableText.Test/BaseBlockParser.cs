using System.Linq;
using Xunit;

namespace NHI.PortableText.Test
{
    /// <summary>
    /// Tests of abstract BaseBlockParser via implementations
    /// </summary>
    public class BaseBlockParser
    {
        private IBlockConverter _converter;

        public BaseBlockParser()
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
            var pt = _converter.SerializeHtml("<div>This should be parsed</div>");
            Assert.Contains("This should be parsed", pt);
        }

        [Fact]
        public void Should_handle_nested_blocks()
        {
            var bc = _converter.ConvertHtml("<div>This should <div>NEW</div> be parsed</div>");
            Assert.Equal(3,bc.Count);
            Assert.Contains("This should", bc[0].Children[0].Text);
            Assert.Contains("NEW", bc[1].Children[0].Text);
            Assert.Contains("be parsed", bc[2].Children[0].Text);
        }

        [Fact]
        public void Should_handle_nested_spans()
        {
            var bc = _converter.ConvertHtml("<div>This should <div>NEW<span>inner</span></div> be parsed</div>");
            Assert.Equal(3, bc.Count);
            Assert.Contains("This should", bc[0].Children[0].Text);
            Assert.Contains("NEW", bc[1].Children[0].Text);
            Assert.Contains("inner", bc[1].Children[1].Text);
            Assert.Contains("be parsed", bc[2].Children[0].Text);
        }

        [Fact]
        public void Should_handle_awkward_nesting()
        {
            var bc = _converter.ConvertHtml("<div>1<div>2<div>3<span>4</span>5</div>6<b>7</b></div>8</div>9");
            var nums = bc.SelectMany(p => p.Children.Select(p => p.Text));
            Assert.Equal("123456789", string.Join("", nums));
        }
    }
}

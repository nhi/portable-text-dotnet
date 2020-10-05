using System.Linq;
using Xunit;

namespace NHI.PortableText.Test
{
    /// <summary>
    /// Tests of various HTML snippets
    /// </summary>
    public class Snippets
    {
        private IBlockConverter _converter;

        public Snippets()
        {
            var settings = new BlockConverterSettings
            {
                MissingParserBehavior = MissingParserBehavior.ThrowException,
                StraySpanBehavior = StraySpanBehavior.AddToRoot,
            };

            _converter = new PortableText.BlockConverter(settings);
        }

        [Fact]
        public void Should_handle_snippet_1()
        {
            var bc = _converter.ConvertHtml(
                @"<p><a href='link1'>a<strong>b</strong>c</a>
                  <em>d<a href='link2'>e</a></em>f</p>
                 ");

            //block
            Assert.Single(bc);
            Assert.Equal(6, bc[0].Children.Count);
            Assert.NotEmpty(bc[0].Key);
            Assert.Null(bc[0].Level);
            Assert.Null(bc[0].ListItem);
            Assert.Equal("normal", bc[0].Style);
            Assert.Equal("block", bc[0].Type);

            //order of spans
            Assert.Equal("a", bc[0].Children[0].Text);
            Assert.Equal("b", bc[0].Children[1].Text);
            Assert.Equal("c", bc[0].Children[2].Text);
            Assert.Equal("d", bc[0].Children[3].Text);
            Assert.Equal("e", bc[0].Children[4].Text);
            Assert.Equal("f", bc[0].Children[5].Text);

            //marks on spans
            Assert.Single(bc[0].Children[0].Marks);
            Assert.Equal(2, bc[0].Children[1].Marks.Count());
            Assert.Single(bc[0].Children[2].Marks);
            Assert.Single(bc[0].Children[3].Marks);
            Assert.Equal(2, bc[0].Children[4].Marks.Count());
            Assert.Empty(bc[0].Children[5].Marks);

            //markdefs on block
            var keys = bc[0].MarkDefs.Select(p => p.Key).ToList();
            var m1 = keys.Intersect(bc[0].Children[1].Marks).Count();
            var m2 = keys.Intersect(bc[0].Children[4].Marks).Count();
            Assert.Equal(1, m1);
            Assert.Equal(1, m2);
        }

        [Fact]
        public void Should_handle_snippet_2()
        {
            var bc = _converter.ConvertHtml(
                @"<p>Paragraph</p>
                  <h1>H1</h1>
                  <h2>H2</h2>
                  <h3>H3</h3>
                  <h4>H4</h4>
                  <h5>H5</h5>
                  <h6>H6</h6>
                  <blockquote>Blockquote</blockquote>
                 ");

            //blocks
            Assert.Equal(8, bc.Count);
            Assert.Equal("normal", bc[0].Style);
            Assert.Equal("h1", bc[1].Style);
            Assert.Equal("h2", bc[2].Style);
            Assert.Equal("h3", bc[3].Style);
            Assert.Equal("h4", bc[4].Style);
            Assert.Equal("h5", bc[5].Style);
            Assert.Equal("h6", bc[6].Style);
            Assert.Equal("blockquote", bc[7].Style);

            //spans
            Assert.Single(bc[0].Children);
            Assert.Single(bc[1].Children);
            Assert.Single(bc[2].Children);
            Assert.Single(bc[3].Children);
            Assert.Single(bc[4].Children);
            Assert.Single(bc[5].Children);
            Assert.Single(bc[6].Children);
            Assert.Single(bc[7].Children);
        }

        [Fact]
        public void Should_handle_snippet_3()
        {
            var bc = _converter.ConvertHtml(
                @"<div><html>1<body>2<pre>3<table>4<tbody>5<tr>6<td>7<ul>8<ol>9<dl>A<span>B<label>C<blockquote>D<div>E<p>F<li>G<dt>H<dd>I</dd></dt></li></p></div></blockquote></label></span></dl></ol></ul></td></tr></tbody></table></pre></body></html></div>");
            var nums = bc.SelectMany(p => p.Children.Select(p => p.Text));

            Assert.Equal("123456789ABCDEFGHI", string.Join("",nums));
        }

    }
}


# NHI.PortableText

NHI.PortableText is a .NET Standard 2.0 library for converting HTML to [PortableText](https://github.com/portabletext/portabletext). The implementation creates PortableText which adheres to the v.0.0.1 working draft specification, and tries to be compliant with existing tooling which deviate from the specification. The library is extendable by adding custom parsers for HTML elements which need to be handled in a project-specific or otherwise non-standard fashion. 

The library uses [HtmlAgilityPack](https://html-agility-pack.net/) to parse HTML elements and [Json.Net](https://www.newtonsoft.com/json) to serialize the internal models to PortableText.

## Quickstart

Example:

```csharp
var bc = new BlockConverter();
var portableText = bc.SerializeHtml("<p>Welcome to <b>PortableText</b></p>");
Console.WriteLine(portableText);
```

Output:

```json
[
  {
    "_type": "block",
    "_key": "5E33484E",
    "markDefs": [],
    "style": "normal",
    "level": null,
    "listItem": null,
    "children": [
      {
        "_type": "span",
        "_key": "C3B531D",
        "marks": [],
        "text": "Welcome to "
      },
      {
        "_type": "span",
        "_key": "2767C23D",
        "marks": [
          "strong"
        ],
        "text": "PortableText"
      }
    ]
  }
]
```

## Configuration

The `BlockConverter` can be configured with an optional `BlockConverterSettings` object. The following settings can be customized:

 - `JsonSerializerSettings`
	 - Used to specify how the PortableText models are serialized to a JSON string. Default behavior is to ignore null values and produce output with no formatting.
	 
- `MissingParserBehavior`
	- An enum which specifies how to handle HTML elements where no matching parser can be found. Default behavior is to throw an exception when this occurs.
	
- `StraySpanBehavior`
	- An enum which determines what to do when malformed HTML results in a span element without a parent block (IE: `<body>this is not valid markup</body>`). Default behavior is to create a block for such elements and add them to the root.


## Registering as a service

When used as a service, the `BlockConverter` should be configured once and registered as a singleton in the DI container.

Example:

```csharp
services.AddSingleton<IBlockConverter>(s =>
{
    var settings = new BlockConverterSettings
    {
        JsonSerializerSettings = new JsonSerializerSettings 
        {
            NullValueHandling = NullValueHandling.Include,
            Formatting = Formatting.Indented
        }
    };
	
    return new BlockConverter(settings);
});
```
## Customizing

Existing parsers can be removed with the `BlockConverter.RemoveParser()` method. Custom parsers can be added with `BlockConverter.AddParser()`.  When a parser is added it will be inserted at the top of the evaluation chain and override any existing parsers which handle the same element.
All custom parsers will be instanciated only once and should be able to function as a singleton.

Example:

```csharp
var bc = new BlockConverter();
bc.RemoveParser(typeof(ImgNodeParser));
bc.AddParser(new MyParser());
```

**NOTE**: Since it is possible for multiple parsers to handle the same element, the order in which the parsers are registered matters. If one parser looks for any element with `class="foo"` and another parser handles `<img>` elements, only the last one registered will run for the element `<img class="foo" src=""/>`.

It is also possible to register parsers automatically. The `BlockConverter` will then scan the `AppDomain` for types that implement the `IHtmlNodeParser` interface. 

Example:

```csharp
var bc = new BlockConverter();
bc.AddParsers();
```
The order of these discovered parsers can be manipulated by decorating the type definition with an `Order` attribute.

## Writing custom parsers

A custom parser can be created by inheriting an abstract `BaseParser` in the library, or implementing the `IHtmlNodeParser` interface.

**BaseParser**

The `BaseParser` can be inherited when there is no need to do any specific operation on the element, but all child nodes need to be handled.

Example:

```csharp
public class HtmlNodeParser : BaseParser
{
	public override bool IsParserFor(HtmlNode n) => n.Name == "html";
}
```

**BaseBlockParser**

The `BaseBlockParser` will parse all child nodes and attach the created blockdata to the root. If there are nested blocks or stray spans, the block will be broken into multiples and the nested elements are inserted into the root in the proper order.

Example:

 ```csharp
public class BlockQuoteParser : BaseBlockParser
{
    public override bool IsParserFor(HtmlNode n) => n.Name == "blockquote";
	protected override BlockModel CreateBlock(HtmlNode node) => new BlockModel(style: "blockquote");
}
```

**BaseDecoratorParser**

The `BaseDecoratorParser`  creates a block where a `MarkDef`  is set on the span and makes sure all child spans that are created gets the `MarkDef` as well.

Example:

```csharp
public class SupNodeParser : BaseDecoratorParser
{
    public override bool IsParserFor(HtmlNode n) => n.Name == "sup";
    protected override string GetMark(HtmlNode n) => "superscript";
}
```

**BaseDropParser**

The `BaseDropParser` will do no operation on the element and make sure that none of the child elements are parsed.

Example:

```csharp
public class HeadNodeParser : BaseDropParser
{
    public override bool IsParserFor(HtmlNode n) => n.Name == "head";
}
```

**BaseSpanParser**

The `BaseSpanParser` adds a span to the parent block and parses all children.

Example:

```csharp
public class BrNodeParser : BaseSpanParser
{
	public override bool IsParserFor(HtmlNode n) => n.Name == "br";
    protected override SpanModel CreateSpan(HtmlNode node) => new SpanModel(text: "\n");
}
```

**Implementing the `IHtmlNodeParser` interface**

If none of the base parsers are sufficient, it is possible to implement the `IHtmlNodeParser` interface directly.

```csharp
public interface IHtmlNodeParser
{
    bool IsParserFor(HtmlNode node);
    void Parse(IBlockConverter converter, List<BlockModel> root, BlockModel parent, HtmlNode node);
}
```

The `IsParserFor` method should evaluate the `HtmlNode` and return true if this parser is able to handle the given node.

The `Parse` method consists of the following parameters:

- `IBlockConverter converter`, used to parse child elements that are not handled in this parser specifically.
- `List<BlockModel> root` is the collection of blocks that have been generated so far. Any new blocks created should generally be added at the end of this list.
- `BlockModel parent` is the block created by the calling parser.  New spans should be added here.
- `HtmlNode node` is the node which should be parsed.

Example:

```csharp
 /// <summary>
 /// An example parser for <A> elements
 /// Note: A link parser is already part of the library and the default parsers, this is just
 /// an example.
 /// </summary>
public class HrefParser : IHtmlNodeParser
{
    public virtual bool IsParserFor(HtmlNode n) => n.Name == "a";
    
    public virtual void Parse(IBlockConverter builder, List<BlockModel> root, BlockModel parent, HtmlNode node)
    {           
        // Create an unique mark for this link
        var mark = BlockUtilities.GenerateKey();

        // Create the MarkDef for this link
        var markDef = new MarkDefModel
        {
            Key = mark,
            Type = "link",
            ["href"] = node.GetAttributeValue<string>("href", string.Empty)
        };

        // Add the MarkDef to the parent
        parent.MarkDefs.Add(markDef);

        // Before child nodes are parsed a temporary block is created to act as a parent.
        // This is done so any childnodes that are created can be marked as being part of 
        // the link before getting added to the real parent.
        var tempBlock = new BlockModel();

        // Parse all children of this node, passing in the temporary block as their parent
        foreach (var child in node.ChildNodes)
        {
            builder.ParseNode(root, tempBlock, child);
        }

        // Any children created on the temporary block are a part of this link and their 
        // spans need to be marked as such. Once marked they are added to the real parent.
        foreach (var child in tempBlock.Children)
        {
            child.Marks.Add(mark);               
            parent.Children.Add(child);
        }

        // Make sure any MarkDefs created on the temporary block also gets passed on to 
        // the real parent
        parent.MarkDefs.AddRange(tempBlock.MarkDefs);
    }
}
```

For further documentation, see source.
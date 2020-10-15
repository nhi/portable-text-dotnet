
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

For further information on usage, see the [package README](https://github.com/nhi/portable-text-dotnet/tree/master/src/README.md)
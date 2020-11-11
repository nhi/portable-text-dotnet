using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NHI.PortableText.Model;
using NHI.PortableText.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NHI.PortableText
{
    /// <summary>
    /// Converts Html to blockdata
    /// </summary>
    public class BlockConverter : IBlockConverter
    {
        private readonly BlockConverterSettings _settings;

        public List<IHtmlNodeParser> Parsers { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Optional parameter to change default behavior of the converter</param>
        public BlockConverter(BlockConverterSettings settings = null)
        {
            _settings = settings ?? new BlockConverterSettings();

            Parsers = new List<IHtmlNodeParser>();

            GetType()
                .Assembly.GetTypes()
                .Where(p => typeof(IHtmlNodeParser).IsAssignableFrom(p) && !p.IsAbstract).ToList()
                .ForEach(p => Parsers.Add((IHtmlNodeParser)Activator.CreateInstance(p)));
        }

        /// <summary>
        /// Remove a registered parser if one is found
        /// </summary>
        /// <param name="type">The type of the parser to remove</param>
        public void RemoveParser(Type type) => Parsers = Parsers.Where(p => p.GetType() != type).ToList();

        /// <summary>
        /// Add a new parser to the converter
        /// </summary>
        /// <param name="instance">An instance of the parser to add</param>
        public void AddParser(IHtmlNodeParser instance) => Parsers.Insert(0, instance);

        /// <summary>
        /// Add all new parsers found to the converter
        /// </summary>
        /// <param name="assembly">Optional parameter to restrict the search for parsers to a specific assembly</param>
        public void AddParsers(Assembly assembly = null)
        {
            var sources = AppDomain.CurrentDomain
                .GetAssemblies().Where(p => p != GetType().Assembly).ToList();

            if (assembly != null) sources = sources.Where(p => p == assembly).ToList();

            var types = sources.SelectMany(p => p.GetTypes())
                .Where(p => typeof(IHtmlNodeParser).IsAssignableFrom(p) && !p.IsAbstract).ToList();

            var ordered = new List<Tuple<int, Type>>();
            types.ForEach(p => ordered.Add(new Tuple<int,Type>(p.GetCustomAttribute<OrderAttribute>()?.Order ?? 0, p)));

            ordered.OrderBy(p => p.Item1).ToList()
                .ForEach(p => Parsers.Insert(0, (IHtmlNodeParser)Activator.CreateInstance(p.Item2)));
        }

        /// <summary>
        /// Convert a html string to a List of BlockModel
        /// </summary>
        /// <param name="html">The html to parse and format</param>
        /// <returns>The blockdata created by the registered parsers</returns>
        public List<BlockModel> ConvertHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return null;

            var root = new List<BlockModel>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var parent = new BlockModel();
            ParseNode(root, parent, doc.DocumentNode);

            if (parent.Children.Any()) switch (_settings.StraySpanBehavior)
                {
                    case StraySpanBehavior.ThrowException:
                        _settings.Logger?.LogError("Parsing halted due to stray span");
                        throw new Exception("Spans were created without a parent block");

                    case StraySpanBehavior.AddToRoot:
                        _settings.Logger?.LogInformation($"{parent.Children.Count} stray spans added to root");
                        root.Add(parent);
                        break;
                }

            return root;
        }

        /// <summary>
        /// Convert a html string to a string of blockdata
        /// </summary>
        /// <param name="html">The html to parse and format</param>
        /// <returns>The blockdata created by the registered parsers</returns>
        public string SerializeHtml(string html)
        {
            var blocks = ConvertHtml(html);
            return blocks == null ? null : JsonConvert.SerializeObject(blocks, _settings.JsonSerializerSettings);
        }

        /// <summary>
        /// Try to parse a HtmlNode with the registered parsers
        /// </summary>
        /// <param name="root">The list where root level blocks should be added</param>
        /// <param name="parent">The block where spans should be added</param>
        /// <param name="node">The node to be parsed</param>
        public void ParseNode(List<BlockModel> root, BlockModel parent, HtmlNode node)
        {
            var isProcessed = false;

            foreach (var parser in Parsers)
            {
                if (!parser.IsParserFor(node)) continue;

                parser.Parse(this, root, parent, node);
                isProcessed = true;
                break;
            }

            if (!isProcessed) switch (_settings.MissingParserBehavior)
                {
                    case MissingParserBehavior.ThrowException:
                        _settings.Logger?.LogError("Parsing halted due to missing parser");
                        throw new Exception($"No parser found for node {node.XPath}");

                    case MissingParserBehavior.HandleChildren:
                        _settings.Logger?.LogWarning($"No parser found for {node.XPath}, parsing children");
                        var nopParser = new RootNodeParser();
                        nopParser.Parse(this, root, parent, node);
                        break;

                    case MissingParserBehavior.Ignore:
                        _settings.Logger?.LogWarning($"No parser found for {node.XPath}, ignoring children");
                        break;
                }
        }

    }
}

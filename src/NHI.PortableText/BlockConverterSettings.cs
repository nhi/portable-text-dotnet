using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NHI.PortableText
{
    /// <summary>
    /// Settings for BlockConverter
    /// </summary>
    public class BlockConverterSettings
    {
        /// <summary>
        /// The settings used when serializing the block models to Json. 
        /// Default is no formatting and ignore nulls.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        /// <summary>
        /// How to handle tags where no parser is found
        /// </summary>
        public MissingParserBehavior MissingParserBehavior { get; set; }

        /// <summary>
        /// How to handle incorrectly nested html
        /// </summary>
        public StraySpanBehavior StraySpanBehavior { get; set; }

        /// <summary>
        /// When this is set, it will be used to log notable events
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Create with the default settings
        /// </summary>
        public BlockConverterSettings()
        {
            MissingParserBehavior = MissingParserBehavior.ThrowException;
            StraySpanBehavior = StraySpanBehavior.AddToRoot;

            JsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
            };
        }
    }

    /// <summary>
    /// How to handle tags where no parser is found
    /// </summary>
    public enum MissingParserBehavior
    {
        /// Throw an exception
        ThrowException,
        
        /// Make sure all children are handled
        HandleChildren,

        /// Do not parse the node and ignore all children
        Ignore
    }

    /// <summary>
    /// How to handle incorrectly nested html
    /// </summary>
    public enum StraySpanBehavior
    {
        /// Throw an exception
        ThrowException,
        
        /// Add all spans that are created without a proper block to a block at the end
        AddToRoot
    }
}

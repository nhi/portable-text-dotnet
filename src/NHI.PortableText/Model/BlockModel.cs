using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NHI.PortableText.Model
{
    /// <summary>
    /// Poco for blocks, used for all blockdata on the root level
    /// </summary>
    public class BlockModel
    {
        [JsonProperty("_type")]
        [JsonPropertyName("_type")]
        public string Type { get; set; }

        [JsonProperty("_key")]
        [JsonPropertyName("_key")]
        public string Key { get; set; }

        [JsonProperty("markDefs")]
        [JsonPropertyName("markDefs")]
        public virtual List<MarkDefModel> MarkDefs { get; set; }

        [JsonProperty("style")]
        [JsonPropertyName("style")]
        public virtual string Style { get; set; }

        [JsonProperty("level")]
        [JsonPropertyName("level")]
        public virtual int? Level { get; set; }

        [JsonProperty("listItem")]
        [JsonPropertyName("listItem")]
        public virtual string ListItem { get; set; }

        [JsonProperty("children")]
        [JsonPropertyName("children")]
        public virtual List<SpanModel> Children { get; set; }

        /// <summary>
        /// Create a default block model
        /// </summary>
        public BlockModel() : this(null) { }

        /// <summary>
        /// Create a block model with customization options
        /// </summary>
        /// <param name="type">The type of the block, default is 'block'</param>
        /// <param name="style">The style of the block, default is 'normal'</param>
        /// <param name="listItem">Specifies the type of a list item</param>
        /// <param name="level">Specifies the indentation level of a list item</param>
        public BlockModel(string type = null, string style = null, string listItem = null, int? level = null)
        {
            Type = type ?? "block";
            Style = style ?? "normal";
            ListItem = listItem;
            Level = level;
            Key = BlockUtilities.GenerateKey();
            Children = new List<SpanModel>();
            MarkDefs = new List<MarkDefModel>();
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortableText.Model
{
    /// <summary>
    /// Poco for span models used in blocks
    /// </summary>
    public class SpanModel
    {
        [JsonProperty("_type")]
        [JsonPropertyName("_type")]
        public string Type { get; set; }

        [JsonProperty("_key")]
        [JsonPropertyName("_key")]
        public string Key { get; set; }

        [JsonProperty("marks")]
        [JsonPropertyName("marks")]
        public virtual List<string> Marks { get; set; }

        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public virtual string Text { get; set; }

        /// <summary>
        /// Create a default span model
        /// </summary>
        public SpanModel() : this(null) { }

        /// <summary>
        /// Create a span model with customization options
        /// </summary>
        /// <param name="type">The type of the span, default is 'span'</param>
        /// <param name="text">The text to set in the span model</param>
        public SpanModel(string type = null, string text = null)
        {
            Type = type ?? "span";
            Text = text ?? string.Empty;
            Key = BlockUtilities.GenerateKey();
            Marks = new List<string>();
        }
    }
}

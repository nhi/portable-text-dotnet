using System.Collections.Generic;

namespace PortableText.Model
{
    /// <summary>
    /// Poco for MarkDefs set in blocks
    /// </summary>
    public class MarkDefModel : Dictionary<string, string>
    {
        public string Key { get => this["_key"]; set => this["_key"] = value; }
        public string Type { get => this["_type"]; set => this["_type"] = value; }
    }
}

using System;

namespace NHI.PortableText
{
    /// <summary>
    /// Used to order parsers registered via discovery
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute : Attribute
    {
        public int Order { get; private set; }

        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}

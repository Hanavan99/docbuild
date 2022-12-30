using DocumentRenderer.Builders;
using System.Xml;

namespace DocumentRenderer.Elements
{
    internal class List : ContainerElement
    {
        public List() : base(false, true)
        {
        }

        public string? ListType { get; set; }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            ListType = element.GetAttribute("type");
            base.Build(builder, element);
        }
    }
}

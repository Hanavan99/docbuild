using DocumentRenderer.Builders;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public class Text : ContainerElement
    {
        public Text(string text) : base(false, true)
        {
            TextContent = text;
        }
        public string TextContent { get; private set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            IsBold = element.GetAttribute("bold") == "true";
            IsItalic = element.GetAttribute("italic") == "true";
            base.Build(builder, element);
        }

    }
}

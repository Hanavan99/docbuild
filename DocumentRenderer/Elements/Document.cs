using DocumentRenderer.Builders;
using System.Xml;

namespace DocumentRenderer.Elements
{

    public class Document : ContainerElement
    {
        public Document() : base(false, false)
        {
        }

        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Authors { get; set; }
        public bool ShowDate { get; set; }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            Title = element.GetAttribute("title");
            Subtitle = element.GetAttribute("subtitle");
            ShowDate = element.GetAttribute("showDate") == "true";
            base.Build(builder, element);
        }
    }

}

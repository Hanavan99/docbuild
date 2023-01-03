using DocumentRenderer.Builders;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public class Text : ContainerElement
    {
        public Text() : base(false, true)
        {
            //TextContent = text;
            Styles = "";
        }
        //public string TextContent { get; private set; }
        //public bool IsBold { get; set; }
        //public bool IsItalic { get; set; }
        //public bool IsCode { get; set; }
        public string Styles { get; set; }
        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            //IsBold = element.GetAttribute("bold") == "true";
            //IsItalic = element.GetAttribute("italic") == "true";
            //IsCode = element.GetAttribute("code") == "true";
            Styles = element.GetAttribute("style");
            base.Build(builder, element);
        }

    }
}

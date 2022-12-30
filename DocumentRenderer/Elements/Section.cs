using DocumentRenderer.Builders;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public class Section : ContainerElement
    {
        public Section() : base(true, false)
        {
        }
        public string? Title { get; set; }
        public DocumentLocation? Location { get; protected set; }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            Title = element.GetAttribute("title");
            base.Build(builder, element);
        }

        public override void PreRender(DocumentLocator locator)
        {
            Location = locator.RegisterElement(this);
            base.PreRender(locator);
        }
    }
}

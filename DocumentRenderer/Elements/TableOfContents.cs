using DocumentRenderer.Builders;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public class TableOfContents : Element
    {
        public TableOfContents() : base(false)
        {
        }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            
        }
    }
}

using DocumentRenderer.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public sealed class LineBreak : Element
    {
        public LineBreak() : base(false)
        {
        }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            // do nothing
        }
    }
}

using DocumentRenderer.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public sealed class RawText : Element
    {
        public RawText(string text) : base(false)
        {
            Text = text;
        }

        public string Text { get; }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            throw new ApplicationException("This element should not be used in the Build() method.");
        }
    }
}

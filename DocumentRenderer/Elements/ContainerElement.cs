using DocumentRenderer.Builders;
using DocumentRenderer.Renderers;
using System.Text.RegularExpressions;
using System.Xml;

namespace DocumentRenderer.Elements
{
    /// <summary>
    /// Represents an element that can have multiple arbitrary children.
    /// </summary>
    public abstract class ContainerElement : Element
    {
        public ContainerElement(bool isLocatable, bool isContainerOnly) : base(isLocatable)
        {
            Children = new List<Element>();
            IsContainerOnly = isContainerOnly;
        }

        public bool IsContainerOnly { get; private set; }

        public List<Element> Children { get; private set; }

        public override void Build(DocumentBuilder builder, XmlElement element)
        {
            foreach (XmlNode child in element.ChildNodes)
            {
                if (child is XmlElement childElement)
                {
                    Element e = builder.Create(child.Name);
                    Children.Add(e);
                    e.Build(builder, childElement);
                }
                else if (child is XmlText textElement)
                {
                    string text = textElement.Value ?? "";
                    text = Regex.Replace(text, "\\A\\s|\\s+|\\s\\z", " ");
                    Children.Add(new RawText(text ?? ""));
                }
                else
                {
                    throw new ApplicationException($"Unknown XML node type \"{child.GetType().Name}\".");
                }
            }
        }

        public override void PreRender(DocumentLocator locator)
        {
            base.PreRender(locator);
            if (!IsContainerOnly) { locator.BeginSection(); }
            foreach (Element child in Children)
            {
                child.PreRender(locator);
                if (child.IsLocatable) { locator.Increment(); }
            }
            if (!IsContainerOnly) { locator.EndSection(); }
        }

        //public override void Render(DocumentRendererBase renderer)
        //{
        //    base.Render(renderer);

        //}

    }
}

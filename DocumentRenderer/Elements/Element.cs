using DocumentRenderer.Builders;
using DocumentRenderer.Renderers;
using System.Xml;

namespace DocumentRenderer.Elements
{
    public abstract class Element
    {
        public Element(bool isLocatable)
        {
            IsLocatable = isLocatable;
        }

        public bool IsLocatable { get; private set; }

        /// <summary>
        /// Populates this element's properties from the given XML element.
        /// </summary>
        /// <param name="builder">An instance of the document builder.</param>
        /// <param name="element">The XML element to be built.</param>
        public abstract void Build(DocumentBuilder builder, XmlElement element);

        /// <summary>
        /// Called before the document is rendered.
        /// </summary>
        /// <param name="locator">An instance of a document locator.</param>
        public virtual void PreRender(DocumentLocator locator)
        {
            // do nothing by default
        }

        ///// <summary>
        ///// Called when the document is being rendered to its target format.
        ///// </summary>
        ///// <param name="builder">An instance of the document renderer.</param>
        //public virtual void Render(DocumentRendererBase renderer)
        //{
        //    // do nothing by default
        //}

    }
}

using DocumentRenderer.Elements;
using DocumentRenderer.Renderers;
using System.Xml;

namespace DocumentRenderer.Builders
{
    public class DocumentBuilder
    {
        public DocumentBuilder(DocumentRendererBase context)
        {
            Context = context;

            RegisterFactory("document", () => new Document());
            RegisterFactory("section", () => new Section());
            RegisterFactory("text", () => new Text(""));
            RegisterFactory("tableofcontents", () => new TableOfContents());
            RegisterFactory("list", () => new List());
        }

        protected readonly Dictionary<string, FactoryDelegate> factories = new();

        public DocumentRendererBase Context { get; set; }

        public Element Create(string name)
        {
            return factories[name].Invoke();
        }

        public void RegisterFactory(string name, FactoryDelegate factory)
        {
            factories[name] = factory;
        }

        public delegate Element FactoryDelegate();
    }

    /*
    public class DocumentBuilder<T> : DocumentBuilder where T : RenderContext
    {
        public DocumentBuilder(T context) : base(context)
        {
           
        }

        #region Builders

        private TableOfContents BuildTableOfContents(XmlElement element)
        {
            return new TableOfContents();
        }

        private Text BuildText(XmlElement element)
        {
            return new Text("") { IsBold = element.GetAttribute("bold") == "true", IsItalic = element.GetAttribute("italic") == "true" };
        }
        #endregion

        public new T Context { get => (T)base.Context; set => base.Context = value; }

        //public void Render<U>(U element, T context) where U : Element
        //{

        //}

        public void RegisterBuilder<U>(string name, BuildDelegate<U> builder) where U : Element
        {
            builders[name] = (element) => builder.Invoke(element);
        }

        public void RegisterRenderer<U>(RenderDelegate<U> renderer) where U : Element
        {
            renderers[typeof(U)] = (element, context) => renderer.Invoke((U)element, (T)context);
        }

        public delegate U BuildDelegate<out U>(XmlElement element) where U : Element;

        public delegate void RenderDelegate<in U>(U element, T context) where U : Element;
    }*/
}

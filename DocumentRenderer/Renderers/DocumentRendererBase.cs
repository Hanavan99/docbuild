using DocumentRenderer.Elements;
using System.Text;

namespace DocumentRenderer.Renderers
{
    public abstract class DocumentRendererBase
    {
        protected readonly Dictionary<Type, Action<Element>> renderers = new();

        public DocumentRendererBase(int depth, DocumentLocator locator)
        {
            Depth = depth;
            Locator = locator;
            RegisterRenderers();
        }

        public int Depth { get; set; }

        public DocumentLocator Locator { get; private set; }

        public abstract void HandleRenderError(Exception e);

        public abstract void RegisterRenderers();

        public void Render(Element element, int depthOffset = 0)
        {
            Depth += depthOffset;
            renderers[element.GetType()].Invoke(element);
            Depth -= depthOffset;
        }

        public void Render<T>(T element, int depthOffset = 0) where T : Element
        {
            Depth += depthOffset;
            renderers[typeof(T)].Invoke(element);
            Depth -= depthOffset;
        }

        //public void RenderBase(Element element)
        //{
        //    if (element.GetType().BaseType is Type bt && bt != typeof(Element))
        //    {
        //        renderers[bt].Invoke(element);
        //    }
        //}

        public void RegisterRenderer<T>(RenderDelegate<T> renderer) where T : Element
        {
            renderers[typeof(T)] = (element) => renderer.Invoke((T)element);
        }

        public delegate void RenderDelegate<T>(T element) where T : Element;
    }
}

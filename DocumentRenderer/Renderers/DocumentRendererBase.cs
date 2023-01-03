using DocumentRenderer.Elements;
using System.Text;

namespace DocumentRenderer.Renderers
{
    public abstract class DocumentRendererBase
    {
        protected readonly Dictionary<Type, Action<Element>> renderers = new();

        public DocumentRendererBase(DocumentLocator locator)
        {
            Locator = locator;
            RegisterRenderers();
        }

        public DocumentLocator Locator { get; private set; }

        public abstract void HandleRenderError(Type type, Exception exception);

        public abstract void RegisterRenderers();

        public void Render(Element element)
        {
            try
            {
                renderers[element.GetType()].Invoke(element);
            }
            catch (Exception e)
            {
                HandleRenderError(element.GetType(), e);
            }
        }

        public void Render<T>(T element) where T : Element
        {
            try
            {
                renderers[typeof(T)].Invoke(element);
            }
            catch (Exception e)
            {
                HandleRenderError(typeof(T), e);
            }
        }

        public void RegisterRenderer<T>(RenderDelegate<T> renderer) where T : Element
        {
            renderers[typeof(T)] = (element) => renderer.Invoke((T)element);
        }

        public delegate void RenderDelegate<T>(T element) where T : Element;
    }
}

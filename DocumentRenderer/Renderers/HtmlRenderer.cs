using DocumentRenderer.Builders;
using DocumentRenderer.Elements;
using System.Text;

namespace DocumentRenderer.Renderers
{
    public class HtmlRenderer : DocumentRendererBase
    {
        public HtmlRenderer(int depth, DocumentLocator locator, StringBuilder sb) : base(depth, locator)
        {
            SB = sb;
        }

        private StringBuilder SB { get; set; }

        public void AppendLine(string s)
        {
            AppendLine(s, 0);
        }

        public void AppendLine(string s, int depthOffset)
        {
            if (Depth + depthOffset >= 0)
            {
                for (int i = 0; i < Depth + depthOffset; i++)
                {
                    SB.Append("    ");
                }
                SB.AppendLine(s);
            }
            else
            {
                throw new ArgumentException("Depth must be >= 0.");
            }
        }

        public override void HandleRenderError(Exception e)
        {
            AppendLine($"<div class=\"error\"><div class=\"error-title\"><strong>ERROR:</strong> Failed to render element \"{GetType().Name}\": {e.Message}</div>{e.StackTrace?.Replace("\n", "<br/>")}</div>");
        }

        public override void RegisterRenderers()
        {
            RegisterRenderer<Element>((e) => Console.WriteLine($"Rendering element \"{e.GetType().Name}\"."));
            RegisterRenderer<ContainerElement>(RenderContainerElement);
            RegisterRenderer<Text>(RenderText);
            RegisterRenderer<TableOfContents>(RenderTableOfContents);
            RegisterRenderer<Section>(RenderSection);
            RegisterRenderer<Document>(RenderDocument);
            RegisterRenderer<List>(RenderList);
        }

        private void RenderList(List element)
        {
            if (element.ListType == "ordered") AppendLine("<ol>");
            foreach (Element child in element.Children)
            {
                AppendLine("<li>", 1);
                Render(child, 2);
                AppendLine("</li>", 1);
            }
            if (element.ListType == "ordered") AppendLine("</ol>");
        }

        private void RenderContainerElement(ContainerElement element)
        {
            foreach (Element child in element.Children)
            {
                Render(child);
            }
            Render<Element>(element);
        }

        private void RenderDocument(Document document)
        {
            if (document.Title != null)
            {
                AppendLine("<div class=\"header\">");
                if (document.Title != null && document.Title.Trim() != "") { AppendLine($"<h1>{document.Title}</h1>", 1); }
                if (document.Subtitle != null && document.Subtitle.Trim() != "") { AppendLine($"<h2>{document.Subtitle}</h2>", 1); }
                if (document.ShowDate) { AppendLine($"<span style=\"margin-top: 2rem; display: inline-block;\">{DateTime.Now:MMMM d, yyyy}</span>", 1); }
                AppendLine("</div>");
            }
            Render<ContainerElement>(document);
        }

        private void RenderSection(Section section)
        {
            AppendLine($"<div id=\"{section.Location?.Identifier}\">");
            if (section.Title != null)
            {
                switch (section.Location?.Depth)
                {
                    case 1:
                        AppendLine($"<h2>{section.Location?.DisplayName}&nbsp;&nbsp;&nbsp;{section.Title}</h2>", 1);
                        break;
                    case 2:
                        AppendLine($"<h3>{section.Location?.DisplayName}&nbsp;&nbsp;&nbsp;{section.Title}</h3>", 1);
                        break;
                    case 3:
                        AppendLine($"<h4>{section.Location?.DisplayName}&nbsp;&nbsp;&nbsp;{section.Title}</h4>", 1);
                        break;
                }
            }
            Render<ContainerElement>(section, 1);
            AppendLine("</div>");
        }

        private void RenderText(Text text)
        {
            string cssClass = "";
            if (text.IsBold) cssClass += "bold";
            if (text.IsItalic) cssClass += "italic";
            if (cssClass.Length > 0)
            {
                AppendLine($"<span class=\"{cssClass}\">");
            }
            else
            {
                AppendLine("<span>");
            }

            if (text.TextContent.Trim() != "")
            {
                AppendLine(text.TextContent.Trim(), 1);
            }
            Render<ContainerElement>(text, 1);
            AppendLine("</span>");
        }

        private void RenderTableOfContents(TableOfContents contents)
        {
            AppendLine("<div class=\"table-of-contents\">");
            AppendLine("<h2>Contents</h2>", 1);
            foreach (Tuple<Element, DocumentLocation> loc2 in Locator)
            {
                string? label = null;
                if (loc2.Item1 is Section s) label = s.Title;
                DocumentLocation loc = loc2.Item2;
                if (loc.Depth == 1)
                {
                    AppendLine($"<div style=\"font-weight: bold;\"><a class=\"table-of-contents-link\" href=\"#{loc.Identifier}\">{loc.DisplayName}&nbsp;&nbsp;{label}</a></div>", 1);
                }
                else
                {
                    AppendLine($"<div style=\"margin-left: {loc.Depth - 1}rem;\"><a class=\"table-of-contents-link\" href=\"#{loc.Identifier}\">{loc.DisplayName}&nbsp;&nbsp;{label}</a></div>", 1);
                }
            }
            AppendLine("</div>");
        }
    }
}

using DocumentRenderer.Builders;
using DocumentRenderer.Elements;
using System.Text;

namespace DocumentRenderer.Renderers
{
    public class HtmlRenderer : DocumentRendererBase
    {

        public HtmlRenderer(string lineIndentString, int depth, DocumentLocator locator, StringBuilder sb) : base(locator)
        {
            LineIndentString = lineIndentString;
            Depth = depth;
            SB = sb;
        }
        public string LineIndentString { get; private set; }
        private int Depth { get; set; }
        private int Inline { get; set; }

        private StringBuilder SB { get; set; }

        public override void HandleRenderError(Type type, Exception exception)
        {
            Write($"<div class=\"error\"><div class=\"error-title\"><strong>ERROR:</strong> Failed to render element \"{type.Name}\": {exception.Message}</div><div class=\"error-details\">{exception.StackTrace?.Replace("\n", "<br/>")}</div></div>");
        }

        public override void RegisterRenderers()
        {
            RegisterRenderer<Element>(e => Console.WriteLine($"Rendering element \"{e.GetType().Name}\"."));
            RegisterRenderer<ContainerElement>(RenderContainerElement);
            RegisterRenderer<Text>(RenderText);
            RegisterRenderer<TableOfContents>(RenderTableOfContents);
            RegisterRenderer<Section>(RenderSection);
            RegisterRenderer<Document>(RenderDocument);
            RegisterRenderer<List>(RenderList);
            RegisterRenderer<RawText>(RenderRawText);
            RegisterRenderer<LineBreak>(RenderLineBreak);
        }

        private void RenderLineBreak(LineBreak element)
        {
            Write("<br>");
        }

        private void RenderRawText(RawText element)
        {
            Write(element.Text);
        }

        private void RenderList(List element)
        {
            if (element.ListType == "ordered") Write("<ol>");
            BeginIndent();
            foreach (Element child in element.Children)
            {
                Write("<li>");
                BeginIndent();
                Render(child);
                EndIndent();
                Write("</li>");
            }
            EndIndent();
            if (element.ListType == "ordered") Write("</ol>");
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
            Write("<!DOCTYPE html>");
            Write("<html>");
            BeginIndent();
            Write("<head>");
            //output.AppendLine("        <link rel=\"stylesheet\" href=\"style.css\">");
            BeginIndent();
            if (document.Title != null && document.Title.Trim() != "") Write($"<title>{document.Title.Trim()}</title>");
            Write("<style>");
            Write(File.ReadAllText("../../../../style.css"));
            Write("</style>");
            EndIndent();
            Write("</head>");
            Write("<body>");
            BeginIndent();
            Write("<div class=\"document\">");
            BeginIndent();
            if (document.Title != null)
            {
                Write("<div class=\"header\">");
                BeginIndent();
                if (document.Title != null && document.Title.Trim() != "") { Write($"<h1>{document.Title}</h1>"); }
                if (document.Subtitle != null && document.Subtitle.Trim() != "") { Write($"<h2>{document.Subtitle}</h2>"); }
                if (document.ShowDate) { Write($"<span style=\"margin-top: 2rem; display: inline-block;\">{DateTime.Now:MMMM d, yyyy}</span>"); }
                EndIndent();
                Write("</div>");
            }
            Render<ContainerElement>(document);
            EndIndent();
            Write("</div>");
            EndIndent();
            Write("</body>");
            EndIndent();
            Write("</html>");
        }

        private void RenderSection(Section section)
        {
            Write($"<div id=\"{section.Location?.Identifier}\">");
            BeginIndent();
            if (section.Title != null)
            {
                switch (section.Location?.Depth)
                {
                    case 1:
                        Write($"<h2>{section.Location?.DisplayName}&nbsp;&nbsp;&nbsp;{section.Title}</h2>");
                        break;
                    case 2:
                        Write($"<h3>{section.Location?.DisplayName}&nbsp;&nbsp;&nbsp;{section.Title}</h3>");
                        break;
                    case 3:
                        Write($"<h4>{section.Location?.DisplayName}&nbsp;&nbsp;&nbsp;{section.Title}</h4>");
                        break;
                }
            }
            Render<ContainerElement>(section);
            EndIndent();
            Write("</div>");
        }

        private void RenderText(Text text)
        {
            string cssClass = text.Styles.Trim();
            //if (text.IsBold) cssClass += "bold ";
            //if (text.IsItalic) cssClass += "italic ";
            //if (text.IsCode) cssClass += "code ";

            BeginInline();
            if (cssClass.Length > 0)
            {
                Write($"<span class=\"{cssClass.Trim()}\">");
            }
            else
            {
                Write("<span>");
            }

            Render<ContainerElement>(text);
            Write("</span>");
            EndInline();
        }

        private void RenderTableOfContents(TableOfContents contents)
        {
            Write("<div class=\"table-of-contents\">");
            BeginIndent();
            Write("<h2>Contents</h2>");
            foreach (Tuple<Element, DocumentLocation> loc2 in Locator)
            {
                string? label = null;
                if (loc2.Item1 is Section s) label = s.Title;
                DocumentLocation loc = loc2.Item2;
                if (loc.Depth == 1)
                {
                    Write($"<div style=\"font-weight: bold;\"><a class=\"table-of-contents-link\" href=\"#{loc.Identifier}\">{loc.DisplayName}&nbsp;&nbsp;{label}</a></div>");
                }
                else
                {
                    Write($"<div style=\"margin-left: {loc.Depth - 1}rem;\"><a class=\"table-of-contents-link\" href=\"#{loc.Identifier}\">{loc.DisplayName}&nbsp;&nbsp;{label}</a></div>");
                }
            }
            EndIndent();
            Write("</div>");
        }

        #region Helper Methods

        public void BeginIndent()
        {
            Depth++;
        }

        public void EndIndent()
        {
            Depth = Math.Max(Depth - 1, 0);
        }

        public void BeginInline()
        {
            if (Inline == 0)
            {
                //SB.AppendLine();
                SB.Append(CreateIndentString(Depth));
            }
            Inline++;
        }

        public void EndInline()
        {
            Inline = Math.Max(Inline - 1, 0);
            if (Inline == 0)
            {
                SB.AppendLine();
            }
        }

        public void Write(string? s)
        {
            if (Inline == 0)
            {
                SB.Append(CreateIndentString(Depth));
                SB.AppendLine(s);
            }
            else
            {
                SB.Append(s);
            }
        }

        private string CreateIndentString(int depth)
        {
            StringBuilder sb = new StringBuilder();
            while (depth > 0)
            {
                sb.Append(LineIndentString);
                depth--;
            }
            return sb.ToString();
        }
        #endregion
    }
}

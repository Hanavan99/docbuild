// See https://aka.ms/new-console-template for more information
using DocumentRenderer.Builders;
using DocumentRenderer.Elements;
using DocumentRenderer.Renderers;
using System.Text;
using System.Xml;

namespace DocumentRenderer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            XmlDocument doc = new XmlDocument();
            doc.Load(new FileStream("../../../../example.xml", FileMode.Open));
            StringBuilder output = new StringBuilder();

            DocumentLocator locator = new DocumentLocator();
            HtmlRenderer renderer = new HtmlRenderer(3, locator, output);
            DocumentBuilder builder = new DocumentBuilder(renderer);


            if (doc.DocumentElement is XmlElement root && root.Name == "document")
            {
                Element e = builder.Create(root.Name);
                e.Build(builder, root);
                e.PreRender(locator);
                output.AppendLine("<!DOCTYPE html>");
                output.AppendLine("<html>");
                output.AppendLine("    <head>");
                output.AppendLine("        <link rel=\"stylesheet\" href=\"style.css\">");
                output.AppendLine("    </head>");
                output.AppendLine("    <body>");
                output.AppendLine("        <div class=\"document\">");
                renderer.Render(e);
                output.AppendLine("        </div>");
                output.AppendLine("    </body>");
                output.AppendLine("</html>");
            }
            else
            {
                throw new Exception("Document root element must be <document>.");
            }
            File.WriteAllText("../../../../output.html", output.ToString());


            //[XmlInclude(typeof(Document))]
            //[XmlInclude(typeof(Section))]
            //[XmlInclude(typeof(TableOfContents))]
            //public abstract class DocElement
            //{
            //    [XmlArray]
            //    public DocElement[] Children { get; set; }
            //}

            //[XmlRoot("document")]
            //public class Document : DocElement
            //{

            //}

            //[XmlType("section")]
            //public class Section : DocElement
            //{

            //}

            //[XmlType("tableofcontents")]
            //public class TableOfContents : DocElement
            //{

            //}
        }
    }
}
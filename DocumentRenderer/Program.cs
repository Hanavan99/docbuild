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
            string path = @"C:\Users\hkuhn\OneDrive - EARP Distribution\Documents\DocBuild";
            FileSystemWatcher fsw = new FileSystemWatcher(path, "*.xml");
            fsw.Changed += (sender, e) =>
            {
                try
                {
                    Thread.Sleep(1000);
                    GenerateDocument(e.FullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };
            fsw.EnableRaisingEvents = true;
            Thread.Sleep(int.MaxValue);
        }

        private static void GenerateDocument(string path)
        {
            XmlDocument doc = new XmlDocument();
            using FileStream fs = new FileStream(path, FileMode.Open);
            doc.Load(fs);
            StringBuilder output = new StringBuilder();

            DocumentLocator locator = new DocumentLocator();
            HtmlRenderer renderer = new HtmlRenderer("    ", 0, locator, output);
            DocumentBuilder builder = new DocumentBuilder(renderer);

            if (doc.DocumentElement is XmlElement root && root.Name == "document")
            {
                Element e = builder.Create(root.Name);
                e.Build(builder, root);
                e.PreRender(locator);
                renderer.Render(e);
            }
            else
            {
                throw new Exception("Document root element must be <document>.");
            }
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(path) ?? ".", Path.GetFileNameWithoutExtension(path) + ".html"), output.ToString());
        }
    }
}
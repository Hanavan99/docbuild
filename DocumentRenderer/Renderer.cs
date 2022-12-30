using DocumentRenderer.Elements;
using System.Collections;
using System.Text;

namespace DocumentRenderer
{





    public class DocumentLocation
    {
        private readonly int[] locations;
        public DocumentLocation(int[] locations)
        {
            this.locations = locations;
        }
        public int Depth => locations.Length;
        public string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (int i in locations)
                {
                    sb.Append(i.ToString());
                    sb.Append(".");
                }

                // remove extra dot at the end
                if (sb.Length > 1)
                {
                    sb.Length--;
                }
                return sb.ToString();
            }
        }

        public string Identifier
        {
            get
            {
                StringBuilder sb = new StringBuilder("location");
                foreach (int i in locations)
                {
                    sb.Append("_");
                    sb.Append(i.ToString());
                }
                return sb.ToString();
            }
        }
    }

    public sealed class DocumentLocator : IEnumerable<Tuple<Element, DocumentLocation>>
    {
        private readonly Stack<int> locationList = new Stack<int>();
        private readonly Dictionary<Element, DocumentLocation> locationMap = new();

        public void BeginSection()
        {
            locationList.Push(1);
        }

        public void EndSection()
        {
            locationList.Pop();
        }

        public void Increment()
        {
            int i = locationList.Pop();
            locationList.Push(i + 1);
        }

        public DocumentLocation RegisterElement(Element element)
        {
            DocumentLocation loc = new DocumentLocation(locationList.ToArray());
            locationMap.Add(element, loc);
            return loc;
        }

        public DocumentLocation GetLocation(Element element)
        {
            return locationMap[element];
        }

        public IEnumerator<Tuple<Element, DocumentLocation>> GetEnumerator()
        {
            return locationMap.Select(o => Tuple.Create(o.Key, o.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return locationMap.Select(o => Tuple.Create(o.Key, o.Value)).GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentRenderer.Elements
{
    public interface IContainerElement
    {
        ICollection<Element> Children { get; }
    }
}

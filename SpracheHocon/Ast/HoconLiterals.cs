using System.Collections.Generic;
using System.Linq;

namespace SpracheHocon.Ast
{
    public class HoconLiterals : HoconValue
    {
        public HoconElement[] Elements { get; private set; }

        public HoconLiterals(IEnumerable<HoconElement> elements)
        {
            elements = elements ?? Enumerable.Empty<HoconElement>();
            Elements = elements.ToArray();
        }

        public override string ToString()
        {
            return string.Join("", Elements.Select(e => e.ToString()));
        }
    }
}

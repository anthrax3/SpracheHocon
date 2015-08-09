using System;
using System.Collections.Generic;
using System.Linq;

namespace SpracheHocon.Ast
{
    public class HoconObject : HoconValue
    {
        public Pair[] Pairs { get; private set; }

        public HoconObject(IEnumerable<Pair> pairs)
        {
            pairs = pairs ?? Enumerable.Empty<Pair>();
            Pairs = pairs.ToArray();
        }

        public override string ToString()
        {
            return "{" + string.Join(Environment.NewLine, Pairs.Select(p => p.ToString())) + "}";
        }
    }
}
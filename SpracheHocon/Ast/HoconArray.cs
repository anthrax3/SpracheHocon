using System.Collections.Generic;
using System.Linq;

namespace SpracheHocon.Ast
{
    public class HoconArray : HoconValue
    {
        public HoconValue[] Values { get; private set; }

        public HoconArray(IEnumerable<HoconValue> values)
        {
            Values = values.ToArray();
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", Values.Select(p => p.ToString())) + "]";
        }
    }
}
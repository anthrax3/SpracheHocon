using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpracheHocon
{
    public class Path
    {
        public IEnumerable<string> Key { get; private set; }

        public Path(IEnumerable<string> key)
        {
            key = key ?? Enumerable.Empty<string>();
            Key = key;
        }

        public override string ToString()
        {
            return string.Join(".", Key);
        }
    }
}

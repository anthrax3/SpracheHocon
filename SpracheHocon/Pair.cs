using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpracheHocon
{
    public class Pair
    {
        public Path Path { get; private set; }
        public Value Value { get; private set; }

        public Pair(Path path, Value value)
        {
            Path = path;
            Value = value;
        }

        public override string ToString()
        {
            return Path + " = " + Value;
        }
    }
}

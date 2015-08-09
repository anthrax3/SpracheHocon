using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpracheHocon
{
    public abstract class Value
    {
    }

    public class HoconObject : Value
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

    public class HoconArray : Value
    {
        public Value[] Values { get; private set; }

        public HoconArray(IEnumerable<Value> values)
        {
            Values = values.ToArray();
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", Values.Select(p => p.ToString())) + "]";
        }
    }

    public class HoconLiteral : Value
    {
        public string Literal { get; private set; }

        public HoconLiteral(string literal)
        {
            Literal = literal;
        }

        public override string ToString()
        {
            return "\"" + Literal + "\"";
        }
    }

    public class HoconSubstitution : Value
    {
        public Path Path { get; private set; }

        public HoconSubstitution(Path path)
        {
            Path = path;
        }

        public override string ToString()
        {
            return "${" + Path + "}";
        }
    }

    public class HoconInclude : Value
    {
        public string File { get; private set; }

        public HoconInclude(string file)
        {
            this.File = file;
        }

        public override string ToString()
        {
            return "{ include \"" + File + "\" }";
        }
    }
}

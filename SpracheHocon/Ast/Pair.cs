namespace SpracheHocon.Ast
{
    public class Pair
    {
        public Path Path { get; private set; }
        public HoconValue Value { get; private set; }

        public Pair(Path path, HoconValue value)
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

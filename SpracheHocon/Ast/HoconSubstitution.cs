namespace SpracheHocon.Ast
{
    public class HoconSubstitution : HoconValue
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
}
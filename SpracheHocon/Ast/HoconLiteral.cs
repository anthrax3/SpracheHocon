namespace SpracheHocon.Ast
{
    public class HoconLiteral : HoconElement
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
}
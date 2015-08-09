namespace SpracheHocon.Ast
{
    public class HoconInclude : HoconValue
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
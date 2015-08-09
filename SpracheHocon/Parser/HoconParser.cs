using System.Collections.Generic;
using Sprache;
using SpracheHocon.Ast;

namespace SpracheHocon.Parser
{
    public static class HoconParser
    {
        private static readonly Parser<char> NotInUnquotedKey = Parse.CharExcept("$\"{}[]:=,#`^?!@*&\\.");
        private static readonly Parser<char> NotInUnquotedText = Parse.CharExcept("$\"{}[]:=,#`^?!@*&\\\r\n");

        public static readonly Parser<string> AssignOp =
            Parse.String(":").Or(Parse.String("=")).Token().Text().Named("AssignOp");

        public static readonly Parser<string> SpaceOrTab =
            Parse.Char(' ').Or(Parse.Char('\t')).Many().Text().Named("SpaceOrTab");

        public static readonly Parser<string> NewLine = Parse.LineEnd;

        public static readonly Parser<string> NewLineToken = Parse.LineEnd.Contained(SpaceOrTab, SpaceOrTab);

        public static readonly Parser<string> NewLineSeparator = NewLineToken.AtLeastOnce().Select(x => string.Join("", x));

        public static readonly Parser<string> CommaSeparator = Parse.String(",").Token().Text();

        public static readonly Parser<string> Separator = NewLineSeparator.Or(CommaSeparator).Named("Separator");

        public static readonly Parser<string> UnquotedKey = NotInUnquotedKey.AtLeastOnce().Token().Text();

        public static readonly Parser<HoconElement> UnquotedString =
            NotInUnquotedText.AtLeastOnce().Token().Text().Select(t => new HoconLiteral(t));

        public static readonly Parser<Path> Path =
            UnquotedKey.Or(Parse.Ref(() => QuotedString))
                .DelimitedBy(Parse.Char('.'))
                .Token()
                .Select(p => new Path(p))
                .Named("Path");

        public static readonly Parser<HoconValue> HoconObject =
            Parse.Ref(() => Pairs)
                .Contained(Parse.Char('{').Token(), Parse.Char('}').Token())
                .Select(pairs => new HoconObject(pairs))
                .Named("HoconObject");

        public static readonly Parser<HoconValue> HoconArray =
            Parse.Ref(() => Value)
                .DelimitedBy(Separator)
                .Contained(Parse.Char('[').Token(), Parse.Char(']').Token()).Select(values => new HoconArray(values))
                .Named("HoconArray");

        public static readonly Parser<HoconValue> HoconLiterals =
            UnquotedString.Or(Parse.Ref(() => Q)).Or(Parse.Ref(() => HoconSubstitution)).AtLeastOnce()
                .Select(l => new HoconLiterals(l))
                .Named("HoconLiterals");

        public static readonly Parser<string> QuotedString =
            Parse.AnyChar.Except(Parse.String("\"")).Many().Text()
                .Contained(Parse.String("\""), Parse.String("\""))
                .Token()
                .Named("QuotedString");

        public static readonly Parser<HoconElement> Q = QuotedString.Select(s => new HoconLiteral(s));

        public static readonly Parser<HoconElement> HoconSubstitution =
            Path
                .Contained(Parse.String("${").Token(), Parse.String("}").Token())
                .Select(p => new HoconSubstitution(p))
                .Named("HoconSubstitution");


        public static readonly Parser<HoconValue> HoconInclude =
            (from _ in Parse.String("include").Token()
             from file in QuotedString
             select file)
                .Contained(Parse.String("{").Token(), Parse.String("}").Token())
                .Select(file => new HoconInclude(file))
                .Named("HoconSubstitution");

        public static readonly Parser<HoconValue> Value =
            HoconInclude
                .Or(HoconObject)
                .Or(HoconLiterals)
                .Or(HoconArray)
                .Named("Value");

        public static readonly Parser<Pair> Pair =
            (from path in Path
             from assignOp in AssignOp
             from value in Value
             select new Pair(path, value))
                .Or(from path in Path
                    from value in HoconObject.Or(HoconInclude)
                    select new Pair(path, value)
                ).Named("Pair");

        public static readonly Parser<IEnumerable<Pair>> Pairs = Pair.DelimitedBy(Separator);
    }
}

using System.Collections.Generic;
using Sprache;
using SpracheHocon.Ast;

namespace SpracheHocon.Parser
{
    public static class HoconParser
    {
        public static readonly Parser<string> AssignOp =
            Parse.String(":").Or(Parse.String("=")).Token().Text().Named("AssignOp");

        public static readonly Parser<string> SpaceOrTab =
            Parse.Char(' ').Or(Parse.Char('\t')).Many().Text().Named("SpaceOrTab");

        public static readonly Parser<string> NewLine = Parse.LineEnd;

        public static readonly Parser<string> NewLineToken = Parse.LineEnd.Contained(SpaceOrTab, SpaceOrTab);

        public static readonly Parser<string> NewLineSeparator = NewLineToken.AtLeastOnce().Select(x => string.Join("", x));

        public static readonly Parser<string> CommaSeparator = Parse.String(",").Token().Text();

        public static readonly Parser<string> Separator = NewLineSeparator.Or(CommaSeparator).Named("Separator");

        public static readonly Parser<Path> Path =
            Parse.Identifier(Parse.LetterOrDigit, Parse.LetterOrDigit)
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

        public static readonly Parser<HoconValue> HoconLiteral =
            Parse.Identifier(Parse.LetterOrDigit, Parse.LetterOrDigit).Token()
                .Select(l => new HoconLiteral(l))
                .Named("HoconLiteral");

        public static readonly Parser<string> QuotedString =
            Parse.AnyChar.Except(Parse.String("\"")).Many().Text()
                .Contained(Parse.String("\""), Parse.String("\""))
                .Token()
                .Named("QuotedString");

        public static readonly Parser<HoconValue> HoconSubstitution =
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
                .Or(HoconLiteral)
                .Or(HoconArray)
                .Or(HoconSubstitution)
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

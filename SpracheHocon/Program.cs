using System;
using Sprache;
using SpracheHocon.Parser;

namespace SpracheHocon
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var str = " \"hej\" ";
            var t = HoconParser.QuotedString.Parse(str);
//            var newLines  = HoconParser.HoconArray.Parse(@"
//   [ 
//  1
//  2
//  3
// ]
//  
// 
// 
//  ");


            var res = HoconParser.HoconObject.Parse(@"
{
    a.b.c.d { e = 7},
    ""foo bar""=1,
    x { 
        include ""bar"" 
    },
    a {
       b {
         c = 123
       }
    }, 
    c = [   1 2 3 ""bar"" ${a.b.c.d.e},
            2,
            3,
            4 
        ],
    d= ${a.b.c.d.e}
}
");
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }

    
}
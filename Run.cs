using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace Odin
{
    public class Run
    {
        public static string Errors { get; set; }

        public Run(GameState gameState, string input)
        {
            //string cad = "n = 4;\r\ncad = \"pan\";\r\ncad2 = \"barras\";\r\ncad = n @@ cad2 @ \" de \"@cad;";
            //string cad = "v = 3;\r\n\r\n{\r\nvar = 5;\r\n\t\r\n\tvar = \"pan\";\r\n\t}\r\n}";
            //string cad = "A=\"A1\";\r\nB=\"B1\";\r\nC=\"C1\";\r\nA=A@\"\";\r\nB=B@\"\";\r\nC=C@\"\";\r\n\r\n{\r\n\r\n  A=\"A2\";\r\n  B=\"B2\";\r\n  A=A@\"\";\r\n  B=B@\"\";\r\n \r\n  { \r\n\r\n   A=\"A3\";\r\n   A=A@\"\";\r\n\r\n   {\r\n\r\n    B=\"B4\";\r\n    C=\"C4\";\r\n    B=B@\"\";\t\r\n    C=C@\"\";\r\n\r\n   }\r\n\r\n   A=A@\"\";\r\n   B=B@\"\";\r\n   C=C@\"\";\r\n\r\n  }\r\n\r\n  A=A@\"\";\r\n  B=B@\"\";\r\n  C=C@\"\";\r\n\r\n}\r\n\r\nA=A@\"\";\r\nB=B@\"\";\r\nC=C@\"\";";
            //string cad = "//while testing\r\na = 1;\r\nwhile(a<5){\r\n\ta *= 2;\r\n}\r\na=a@\"\";";
            //string cad = "//fibonacci\r\n\r\nA=0;\r\nB=1;\r\ncont=10;\r\nwhile(cont){\r\ncont--;\r\nfib = \"fibonacci\";\r\nC = A+B;\r\nA=B;\r\nB=C;\r\n}";
            //string cad = "}";
            //string cad = "v = a;";
            //string cad = "// += *= ...\r\na=9;a++;++a;--a;a*=10;a+=5;a-=2;a/=2;a%=5;a&=1;a|=2;a^=4;a~=2;a>>=1;a<<=1;a@=a;a@@=a;";
            //string cad = "cad = \"pan\"; cad = 5;cad=(-++cad);cad@=\"\";";
            //string cad = "c=0;while(c<5) c++;c@=\"\";";
            //string cad = "a = log(2,1000000000);context.TriggerPlayer;";
            input = NormalizeInput.Normalize(input);

            Scanner S = new Scanner(input);
            List<Token> lista = new List<Token>();
            lista = S.ScanTokens();
            //foreach (var token in lista) WriteLine(token);

            Parser<object> parser = new Parser<object>(lista);
            List<Stmt<object>> stmts = parser.Parse();

            Interpreter interpreter = new Interpreter(gameState);
            interpreter.Interpret(stmts);
        }
    }
}

using Odin;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Markup;
using static System.Console;
using static System.Math;

namespace Odin
{
    class Program
    {
        static void Main(string[] args)
        {
            //string cad = "n = 3 ;\r\ncad = \"pan\";\r\ncad2 = \"barras\";\r\ncad = n @@ cad2 @ \" de \"@cad;";
            //string cad = "v = 3;\r\n\r\n{\r\nvar = 5;\r\n\t{\r\n\tvar = \"pan\";\r\n\t}\r\n}";
            //string cad = "A=\"A1\";\r\nB=\"B1\";\r\nC=\"C1\";\r\nA=A@\"\";\r\nB=B@\"\";\r\nC=C@\"\";\r\n\r\n{\r\n\r\n  A=\"A2\";\r\n  B=\"B2\";\r\n  A=A@\"\";\r\n  B=B@\"\";\r\n \r\n  { \r\n\r\n   A=\"A3\";\r\n   A=A@\"\";\r\n\r\n   {\r\n\r\n    B=\"B4\";\r\n    C=\"C4\";\r\n    B=B@\"\";\t\r\n    C=C@\"\";\r\n\r\n   }\r\n\r\n   A=A@\"\";\r\n   B=B@\"\";\r\n   C=C@\"\";\r\n\r\n  }\r\n\r\n  A=A@\"\";\r\n  B=B@\"\";\r\n  C=C@\"\";\r\n\r\n}\r\n\r\nA=A@\"\";\r\nB=B@\"\";\r\nC=C@\"\";";
            //string cad = "//while testing\r\na = 0;\r\nwhile(false){\r\n\ta = a + 1;\r\n}\r\na=a@\"\";";
            string cad = "//fibonacci\r\n\r\nA=0;\r\nB=1;\r\ncont=10;\r\nwhile(cont>0){\r\ncont = cont - 1;\r\nfib = \"fibonacci\";\r\nC = A+B;\r\nA=B;\r\nB=C;\r\n}";
            cad = NormalizeInput.Normalize(cad);

            Scanner S = new Scanner(cad);
            List<Token> lista = new List<Token>();
            lista = S.ScanTokens();
            //foreach (var token in lista) WriteLine(token);

            Parser<object> parser = new Parser<object>(lista);
            List<Stmt<object>> stmts = parser.Parse();

            Interpreter interpreter = new Interpreter();
            Write(cad + " = ");
            interpreter.Interpret(stmts);
        }
    }
}
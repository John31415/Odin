using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;
using static System.Math;

namespace Odin
{
    internal class ErrorReporter
    {
        private static void ReportError(int line, int column, string errorLine, string message)
        {
            int startIndex = Max(0, column - 40);
            int length = Min(errorLine.Length - startIndex, 80);
            errorLine = "... " + errorLine.Substring(startIndex, length) + " ...";
            string error = "\nError: " + message + "\n   (" + line + ", " + column + ") | " + errorLine + "\n";
            column = column - startIndex + line.ToString().Length + column.ToString().Length + 14;
            while (column > 0)
            {
                error += " ";
                column--;
            }
            error += "^-- Here.\n";

            //print error
            WriteLine(error);
        }

        private static void Error(int line, int column, string errorLine, string message)
        {
            ReportError(line, column, errorLine, message);
        }

        private static string ErrorLine(int current, int lineBeginning)
        {
            string _source = Scanner.Source;
            int iter = current - 1;
            for (int i = 0; i < 100 && iter < _source.Length - 1 && _source[iter] != '\n'; i++) iter++;
            return _source.Substring(lineBeginning, iter - lineBeginning + 1);
        }

        internal static void ThrowError(string message, int line, int current, int lineBeginning)
        {
            Error(line, current - lineBeginning - 1, ErrorLine(current, lineBeginning), message);
        }
    }
}

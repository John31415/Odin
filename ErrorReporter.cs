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

        public static void Error(int line, int column, string errorLine, string message)
        {
            ReportError(line, column, errorLine, message);
        }
    }
}

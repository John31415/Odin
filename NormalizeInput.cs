using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class NormalizeInput
    {
        public static string Normalize(string input)
        {
            string aux = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] != '\r') aux += input[i]; 
            }
            return aux;
        }
    }
}

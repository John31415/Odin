using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Odin
{
    internal class Environment
    {
        //dad
        private Environment enclosing;
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public Environment()
        {
            enclosing = null!;
        }

        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name._lexeme))
            {
                return values[name._lexeme];
            }
            if (enclosing != null) return enclosing.Get(name);
            ThrowError(name, $"Undefined variable '{name._lexeme}'.");
            return null!;
        }

        public void Define(string name, object value)
        {
            if(!Assign(name, value))
            {
                values.Add(name, value);
            }
        }

        private bool Assign(string name, object value)
        {
            if (values.ContainsKey(name))
            {
                values[name] = value;
                return true;
            }
            if (enclosing != null) return enclosing.Assign(name, value);
            return false;
        }

        private void ThrowError(Token token, string message)
        {
            ErrorReporter.ThrowError(message, token._line, token._position, token._lineBeginning);
        }
    }
}

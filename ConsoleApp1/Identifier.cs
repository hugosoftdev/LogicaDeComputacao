using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Identifier : Node
    {
        public Identifier(Token value)
        {
            this.value = value;
        }

        override public Object Evaluate(SymbolTable st)
        {
            Token token = (Token) this.value;
            string key = (string) token.value;
            return st.Get(key);
        }
    }
}

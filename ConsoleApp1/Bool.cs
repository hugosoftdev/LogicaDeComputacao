using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Bool : Node
    {
        public Bool(Token value)
        {
            this.value = value;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) this.value;
            bool val = bool.Parse(token.value.ToString());
            return new EvaluateReturn() { value = val, type = token.type };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class IntVal : Node
    {
        public IntVal(Token value)
        {
            this.value = value;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) this.value;
            int val = Int32.Parse(token.value.ToString());
            return new EvaluateReturn() { value = val, type = token.type };
        }
    }
}

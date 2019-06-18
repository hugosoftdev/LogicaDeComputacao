using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Type : Node
    {
        public Type(Token value)
        {
            this.value = value;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) this.value;
            return new EvaluateReturn() { value = token.type, type = token.type }; 
        }
    }
}

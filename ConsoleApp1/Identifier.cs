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

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) this.value;
            string key = (string) token.value;
            if((TokenType) st.GetType(key) == TokenType.INTEGER)
            {
                return new EvaluateReturn() { value = st.GetValue(key), type = TokenType.INT };
            }
            return new EvaluateReturn() { value = st.GetValue(key), type = TokenType.BOOL };
        }
    }
}

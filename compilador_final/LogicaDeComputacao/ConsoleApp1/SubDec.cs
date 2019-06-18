using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SubDec : Node
    {
        public SubDec(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) value;
            string key = (string) token.value;
            st.SetType(key, TokenType.SUB);
            st.SetValue(key, this);
            return new EvaluateReturn() { value = null, type = null };
        }
    }
}

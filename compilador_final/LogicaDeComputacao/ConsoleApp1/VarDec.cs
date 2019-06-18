using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class VarDec : Node
    {
        public VarDec(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) this.children.ElementAt(0).value;
            string key = (string) token.value;
            st.SetType(key, (TokenType) this.children.ElementAt(1).Evaluate(st).value);
            return new EvaluateReturn() { value = null, type = null };

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class FuncDec : Node
    {
        public FuncDec(Token value, List<Node> children, Node type)
        {
            this.value = value;
            this.children = children;
            this.type = type;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token)value;
            string key = (string)token.value;
            st.SetType(key, TokenType.FUNCTION);
            st.SetValue(key, this);
            return new EvaluateReturn() { value = null, type = null };
        }
    }
}

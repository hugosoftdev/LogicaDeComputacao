using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Assignment: Node
    {
        public Assignment(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public Object Evaluate(SymbolTable st)
        {
            Token childrenToken = (Token) this.children.ElementAt(0).value;
            string stKey = (string) childrenToken.value;
            int assignmentValue = (int) this.children.ElementAt(1).Evaluate(st);
            st.Set(stKey, assignmentValue);
            return null;
        }
    }
}

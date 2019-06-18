using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Statements : Node
    {
        public Statements(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            foreach(Node node in children)
            {
                node.Evaluate(st);
            }
            return new EvaluateReturn() { value = null, type = null };
        }
    }
}

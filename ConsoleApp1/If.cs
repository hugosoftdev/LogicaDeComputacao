using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class If : Node
    {
        public If(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public Object Evaluate(SymbolTable st)
        {
            bool condition = (bool) children.ElementAt(0).Evaluate(st);
            bool hasElse = children.Count() == 3;
            if (condition)
            {
                children.ElementAt(1).Evaluate(st);
            } else
            {
                if (hasElse)
                {
                    children.ElementAt(2).Evaluate(st);
                }
            }
            return null;
        }
    }
}

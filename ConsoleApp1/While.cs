using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class While : Node
    {
        public While(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public Object Evaluate(SymbolTable st)
        {
            while ((bool) children.ElementAt(0).Evaluate(st))
            {
                children.ElementAt(1).Evaluate(st);
            }
            return 1;
        }
    }
}

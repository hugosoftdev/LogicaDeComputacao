using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Print : Node
    {
        public Print(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public Object Evaluate(SymbolTable st)
        {
            Console.WriteLine(this.children.ElementAt(0).Evaluate(st));
            return null;
        }
    }
}

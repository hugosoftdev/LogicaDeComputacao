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

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            EvaluateReturn evalReturn = this.children.ElementAt(0).Evaluate(st);
            Console.WriteLine(evalReturn.value);
            return new EvaluateReturn() { value = null, type = null };
        }
    }
}

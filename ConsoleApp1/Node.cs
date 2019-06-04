using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public struct EvaluateReturn
    {
        public Object value;
        public Object type;
    }

    public abstract class Node
    {
        public Object value;
        public List<Node> children;
        public Node type;
        public abstract EvaluateReturn Evaluate(SymbolTable st); 
    }
}

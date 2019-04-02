using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public abstract class Node
    {
        public Object value;
        public List<Node> children;
        public abstract Object Evaluate(SymbolTable st); 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class NoOp : Node
    {
        public NoOp()
        {
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            return new EvaluateReturn() { value = null, type = null };
        }
    }
}

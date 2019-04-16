using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Input : Node
    {
        public Input(Token value)
        {
            this.value = value;
        }

        override public Object Evaluate(SymbolTable st)
        {
            return Int32.Parse(Console.ReadLine());
        }
    }
}

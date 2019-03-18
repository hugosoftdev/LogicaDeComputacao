using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class IntVal : Node
    {
        public IntVal(Token value)
        {
            this.value = value;
        }

        override public Object Evaluate()
        {
            Token token = (Token)this.value;
            return token.value;
        }
    }
}

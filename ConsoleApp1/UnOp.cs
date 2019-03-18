using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class UnOp : Node
    {
        public UnOp(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public Object Evaluate()
        {
            Token token = (Token) this.value;
            if(token.type == TokenType.MINUS)
            {
                return - (int) this.children.ElementAt(0).Evaluate();
            } else if (token.type == TokenType.PLUS)
            {
                return (int) this.children.ElementAt(0).Evaluate();
            }
            throw new Exception("Um Nó foi classificado como UnOp sem ser um token do tipo PLUS ou MINUS");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class BinOp : Node
    {
        public BinOp(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public Object Evaluate(SymbolTable st)
        {
            int firstNumber = (int)this.children.ElementAt(0).Evaluate(st);
            int secondNumber = (int)this.children.ElementAt(1).Evaluate(st);

            Token token = (Token) this.value;
            if(token.type == TokenType.MINUS)
            {
                return firstNumber - secondNumber;
            } else if(token.type == TokenType.PLUS)
            {
                return firstNumber + secondNumber;
            } else if(token.type == TokenType.MULTIPLY)
            {
                return firstNumber * secondNumber;
            } else if(token.type == TokenType.DIVIDE)
            {
                return firstNumber / secondNumber;
            }
            throw new Exception("Um Nó foi classificado como BinOp sem ser um token do tipo PLUS | MINUS | MULTIPLY | DIVIDE");
        }
    }
}

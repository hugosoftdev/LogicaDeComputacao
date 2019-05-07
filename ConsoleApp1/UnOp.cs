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

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token token = (Token) this.value;
            EvaluateReturn evalReturn = this.children.ElementAt(0).Evaluate(st);
            if(token.type == TokenType.MINUS && ((TokenType) evalReturn.type == TokenType.INT))
            {
                int value =  - (int) evalReturn.value;
                return new EvaluateReturn() { value = value, type = TokenType.INT };
            } else if (token.type == TokenType.PLUS && ((TokenType)evalReturn.type == TokenType.INT))
            {
                int value = (int)evalReturn.value;
                return new EvaluateReturn() { value = value, type = TokenType.INT };
            }
            else if (token.type == TokenType.NOT && ((TokenType)evalReturn.type == TokenType.BOOL))
            {
                bool value = !(bool)evalReturn.value;
                return new EvaluateReturn() { value = value, type = TokenType.BOOL };
            } else
            {
                throw new Exception("Operação unária com tipos inválidos");
            }
            throw new Exception("Um Nó foi classificado como UnOp sem ser um token do tipo PLUS,MINUS ou NOT");
        }
    }
}

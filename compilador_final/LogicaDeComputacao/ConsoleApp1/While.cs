using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class While : Node
    {
        public While(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            EvaluateReturn evalReturn = children.ElementAt(0).Evaluate(st);
            if((TokenType) evalReturn.type != TokenType.BOOL)
            {
                throw new Exception("While must receiva a expression that returns a bool");
            }
            bool condition = (bool)evalReturn.value;
            while (condition)
            {
                children.ElementAt(1).Evaluate(st);
                evalReturn = children.ElementAt(0).Evaluate(st);
                if ((TokenType)evalReturn.type != TokenType.BOOL)
                {
                    throw new Exception("While must receiva a expression that returns a bool");
                }
                condition = (bool)evalReturn.value;
            }
            return new EvaluateReturn() { value=null, type = null };
        }
    }
}

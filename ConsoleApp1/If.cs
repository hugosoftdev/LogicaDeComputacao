using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class If : Node
    {
        public If(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            EvaluateReturn conditionEval = children.ElementAt(0).Evaluate(st); 
            if((TokenType) conditionEval.type != TokenType.BOOL)
            {
                throw new Exception("Condition must be based on a boolean expression");
            }
            bool condition = (bool) conditionEval.value;
            bool hasElse = children.Count() == 3;
            if (condition)
            {
                children.ElementAt(1).Evaluate(st);
            } else
            {
                if (hasElse)
                {
                    children.ElementAt(2).Evaluate(st);
                }
            }
            return new EvaluateReturn() { value=null, type= null};
        }
    }
}

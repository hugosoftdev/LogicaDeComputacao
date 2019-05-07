using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Assignment: Node
    {
        public Assignment(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            Token childrenToken = (Token) this.children.ElementAt(0).value;
            string stKey = (string) childrenToken.value;
            EvaluateReturn evalReturn =   this.children.ElementAt(1).Evaluate(st);
            TokenType varType = (TokenType) st.GetType(stKey);

            if(varType == TokenType.INTEGER)
            {
                if ((TokenType) evalReturn.type == TokenType.INT)
                {
                    int assignmentValue = (int) evalReturn.value;
                    st.SetValue(stKey, assignmentValue);
                } else
                {
                    throw new Exception("Variable is INT but boolean was given");
                }          
            } else
            {
                if ((TokenType) evalReturn.type == TokenType.BOOL)
                {
                    bool assignmentValue = (bool) evalReturn.value;
                    st.SetValue(stKey, assignmentValue);
                }
                else
                {
                    throw new Exception("Variable is Bool but Integer was given");
                }
            }       
            return new EvaluateReturn() {  value=null, type=null };
        }
    }
}

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
            int labelCounter = st.GetLabelCounter();
            string loopLabel = $"LOOP_{labelCounter}";
            NasmManager.AddLine(loopLabel+ ":");
            EvaluateReturn evalReturn = children.ElementAt(0).Evaluate(st);
            if((TokenType) evalReturn.type != TokenType.BOOL)
            {
                throw new Exception("While must receive a expression that returns a bool");
            }
            NasmManager.AddLine("CMP EBX, False");
            NasmManager.AddLine($"JE EXIT_{labelCounter}");
            bool condition = (bool)evalReturn.value;
            children.ElementAt(1).Evaluate(st);
            NasmManager.AddLine($"JMP {loopLabel}");
            NasmManager.AddLine($"EXIT_{labelCounter}:");
            return new EvaluateReturn() { value=null, type = null };
        }
    }
}

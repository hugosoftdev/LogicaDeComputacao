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
            int labelCounter = st.GetLabelCounter();
            string ifLabel = $"IF_{labelCounter}";
            NasmManager.AddLine(ifLabel + ":");
            EvaluateReturn conditionEval = children.ElementAt(0).Evaluate(st); 
            if((TokenType) conditionEval.type != TokenType.BOOL)
            {
                throw new Exception("Condition must be based on a boolean expression");
            }
            bool condition = (bool) conditionEval.value;
            bool hasElse = children.Count() == 3;

            NasmManager.AddLine("CMP EBX, False");

            if (hasElse)
            {
                NasmManager.AddLine($"JE ELSE_{labelCounter}");
            } else
            {
                NasmManager.AddLine($"JE END_IF_{labelCounter}");
            }

            children.ElementAt(1).Evaluate(st);
            NasmManager.AddLine($"JMP END_IF_{labelCounter}");

            if (hasElse)
            {
                NasmManager.AddLine($"ELSE_{labelCounter}");
                children.ElementAt(2).Evaluate(st);
            }

            NasmManager.AddLine($"END_IF_{labelCounter}:");
            return new EvaluateReturn() { value=null, type= null};
        }
    }
}

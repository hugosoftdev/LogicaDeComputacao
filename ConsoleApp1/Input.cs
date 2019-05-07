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

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            try
            {
                return new EvaluateReturn() { value = Int32.Parse(Console.ReadLine()), type = TokenType.INT };
            } catch (Exception e)
            {
                try
                {
                    return new EvaluateReturn() { value = bool.Parse(Console.ReadLine().ToLower()), type = TokenType.BOOL };
                } catch (Exception e2)
                {
                    throw new Exception("Valor inserido no input não é um token válido");
                }
            }
        }
    }
}

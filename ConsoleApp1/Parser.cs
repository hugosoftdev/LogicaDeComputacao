using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Parser
    {
        public static Tokenizer tokens { get; set; }
        
        public static int parseExpression()
        {
            while (tokens.actual.type != TokenType.EOF)
            {
                tokens.selectNext();         
            }
            return 10;
        }

        public static int run(string input)
        {
            Parser.tokens = new Tokenizer() { origin = input, position = 0, actual = new Token() };
            return parseExpression();
        }
    }
}

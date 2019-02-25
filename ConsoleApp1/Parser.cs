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

            int output = 0;
            output += parseTerm();
            while(tokens.actual.type == TokenType.PLUS || tokens.actual.type == TokenType.MINUS)
            {
                if(tokens.actual.type == TokenType.PLUS)
                {
                    output += parseTerm();

                } else if(tokens.actual.type == TokenType.MINUS)
                {
                    output -= parseTerm();
                }
            }

            if(tokens.actual.type != TokenType.EOF)
            {
                throw new Exception("Entrada inválida, a cadeia terminou sem um EOF");
            }
            return output;
        }

        public static int parseTerm()
        {

            int output = 0;
            tokens.selectNext();
            if (tokens.actual.type == TokenType.INT)
            {
                output += tokens.actual.value;
                tokens.selectNext();
                while (tokens.actual.type == TokenType.MULTIPLY || tokens.actual.type == TokenType.DIVIDE)
                {
                    if (tokens.actual.type == TokenType.MULTIPLY)
                    {
                        tokens.selectNext();
                        if (tokens.actual.type == TokenType.INT)
                        {
                            output *= tokens.actual.value;
                        }
                        else
                        {
                            throw new Exception("Depois do operador * é necessário um número");
                        }
                    }
                    else if (tokens.actual.type == TokenType.DIVIDE)
                    {
                        tokens.selectNext();
                        if (tokens.actual.type == TokenType.INT)
                        {
                            output /= tokens.actual.value;
                        }
                        else
                        {
                            throw new Exception("Depois do operador / é necessário um número");
                        }
                    }
                    tokens.selectNext();
                }
            }
            else
            {
                throw new Exception("A conta deve começar com um número");
            }
            return output;
        }

        public static int run(string input)
        {
            Parser.tokens = new Tokenizer() { origin = input, position = 0, actual = new Token() };
            return parseExpression();
        }
    }
}

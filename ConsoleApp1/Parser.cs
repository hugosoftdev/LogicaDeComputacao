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
                    tokens.selectNext();
                    output += parseTerm();

                } else if(tokens.actual.type == TokenType.MINUS)
                {
                    tokens.selectNext();
                    output -= parseTerm();
                }
            }
            return output;
        }

        public static int parseTerm()
        {
            int output = 0;
            output += parseFactor();
            while (tokens.actual.type == TokenType.DIVIDE || tokens.actual.type == TokenType.MULTIPLY)
            {
                if (tokens.actual.type == TokenType.MULTIPLY)
                {
                    tokens.selectNext();
                    output *= parseFactor();

                }
                else if (tokens.actual.type == TokenType.DIVIDE)
                {
                    tokens.selectNext();
                    output /= parseFactor();
                }
            }
            return output;
        }

        public static int parseFactor()
        {
            int output = 0;       
            if(tokens.actual.type == TokenType.INT)
            {
                output = tokens.actual.value;
                tokens.selectNext();
            }  else if(tokens.actual.type == TokenType.PARENTHESESBEGIN)
            {
                tokens.selectNext();
                output += parseExpression();
                if(tokens.actual.type != TokenType.PARENTHESESEND)
                {
                    throw new Exception("Invalid Syntax - Missing )");
                }
                else
                {
                    tokens.selectNext();
                }
            } else if(tokens.actual.type == TokenType.MINUS)
            {
                tokens.selectNext();
                output -= parseFactor();
            } else if(tokens.actual.type == TokenType.PLUS)
            {
                tokens.selectNext();
                output += parseFactor();
            } else
            {
                throw new Exception("Unnespected Token");
            }
            return output;
        }

        public static int run(string input)
        {
            Parser.tokens = new Tokenizer() { origin = input, position = 0, actual = new Token() };
            tokens.selectNext();
            int output = parseExpression();
            if (tokens.actual.type != TokenType.EOF)
            {
                throw new Exception("Entrada inválida, a cadeia terminou sem um EOF");
            }
            return output;
        }
    }
}

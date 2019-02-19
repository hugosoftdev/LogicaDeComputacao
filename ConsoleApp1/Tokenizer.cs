using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Tokenizer
    {
        public string origin { get; set; }
        public int position { get; set; }
        public Token actual { get; set; }
        private List<char> alphabet = new List<char>() {'0','1','2','3','4','5','6','7','8','9','+','-' };

        public Token selectNext()
        {
            int iterator = position;

            string buffer = "";

            while (iterator < origin.Length)
            {
                if ((!string.IsNullOrEmpty(origin[iterator].ToString()))  && (!string.IsNullOrWhiteSpace(origin[iterator].ToString()))) 
                {

                    if (!alphabet.Contains(origin[iterator]))
                    {
                        throw new Exception(string.Format("Lexical Error -> '{0}' não faz parte do alfabeto", origin[iterator]));
                    }


                    if(buffer.Length > 0)
                    {
                        if (origin[iterator] == '-' || origin[iterator] == '+')
                        {
                            actual.type = TokenType.INT;
                            actual.value =Int32.Parse(buffer);
                            position = iterator;
                            return actual;
                        }
                    }


                    if (Char.IsDigit(origin[iterator]))
                    {
                        buffer += origin[iterator];
                    }

                    if (origin[iterator] == '-')
                    {
                        actual.type = TokenType.MINUS;
                        position = iterator + 1;
                        return actual;
                    }

                    if(origin[iterator] == '+')
                    {
                        actual.type = TokenType.PLUS;
                        position = iterator + 1;
                        return actual;
                    }

                    iterator++;

                } else
                {
                    if(buffer.Length > 0)
                    {
                        actual.type = TokenType.INT;
                        actual.value = Int32.Parse(buffer);
                        position = iterator + 1;
                        return actual;
                    }
                    iterator++;
                }
            }

            if(buffer.Length > 0)
            {
                actual.type = TokenType.INT;
                actual.value = Int32.Parse(buffer);
                position = iterator;
                return actual;
            }

            actual.type = TokenType.EOF;
            return actual;
        }
    }
}

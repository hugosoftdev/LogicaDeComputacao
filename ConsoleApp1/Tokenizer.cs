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
        private List<char> alphabet = new List<char>() {'0','1','2','3','4','5','6','7','8','9','+','-', '*', '/', '(', ')' };

        public Token selectNext()
        {
            int iterator = position;

            string buffer = "";

            while (iterator < origin.Length)
            {
                if ((!string.IsNullOrEmpty(origin[iterator].ToString()))  && (!string.IsNullOrWhiteSpace(origin[iterator].ToString()))) 
                {
                    bool foundWord = false;
                    if(buffer.Length == 0)
                    {
                        if (Char.IsLetter(origin[iterator]) || origin[iterator] == '_')
                        {
                            foundWord = true;
                            buffer += origin[iterator];
                            iterator += 1;
                            while((iterator < origin.Length) && (Char.IsLetterOrDigit(origin[iterator]) || origin[iterator] == '_'))
                            {
                                buffer += origin[iterator];
                                iterator += 1;
                            }
                        }
                    }

                    if (foundWord)
                    {
                        if(buffer == "BEGIN")
                        {
                            actual.type = TokenType.BEGIN;
                        } else if (buffer == "END")
                        {
                            actual.type = TokenType.END;
                        } else if (buffer.ToUpper() == "PRINT")
                        {
                            actual.type = TokenType.PRINT;
                        } else
                        {
                            actual.type = TokenType.IDENTIFIER;
                            actual.value = buffer;
                        }
                        position = iterator;
                        return actual;
                    }


                    if(buffer.Length > 0)
                    {
                        if (origin[iterator] == '-' || origin[iterator] == '+' || origin[iterator] == '*' || origin[iterator] == '/' || origin[iterator] == '(' || origin[iterator] == ')')
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

                    if (origin[iterator] == '/')
                    {
                        actual.type = TokenType.DIVIDE;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == '*')
                    {
                        actual.type = TokenType.MULTIPLY;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == '(')
                    {
                        actual.type = TokenType.PARENTHESESBEGIN;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == ')')
                    {
                        actual.type = TokenType.PARENTHESESEND;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == '=')
                    {
                        actual.type = TokenType.EQUAL;
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
                        if((origin[iterator] == '\n') || origin[iterator] == '\r')
                        {
                            position = iterator;
                        } else
                        {
                            position = iterator + 1;
                        }             
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

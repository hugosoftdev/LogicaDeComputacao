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
        private List<char> alphabet = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '*', '/', '(', ')' };

        public Token selectNext()
        {
            int iterator = position;

            string buffer = "";

            while (iterator < origin.Length)
            {
                if ((!string.IsNullOrEmpty(origin[iterator].ToString())) && (!string.IsNullOrWhiteSpace(origin[iterator].ToString())))
                {
                    bool foundWord = false;
                    if (buffer.Length == 0)
                    {
                        if (Char.IsLetter(origin[iterator]) || origin[iterator] == '_')
                        {
                            foundWord = true;
                            buffer += origin[iterator];
                            iterator += 1;
                            while ((iterator < origin.Length) && (Char.IsLetterOrDigit(origin[iterator]) || origin[iterator] == '_'))
                            {
                                buffer += origin[iterator];
                                iterator += 1;
                            }
                        }
                    }

                    if (foundWord)
                    {
                        if (buffer.ToUpper() == "FIM")
                        {
                            actual.type = TokenType.END;
                        }
                        else if (buffer.ToUpper() == "MOSTRE_ME")
                        {
                            actual.type = TokenType.PRINT;
                        }
                        else if (buffer.ToUpper() == "ENQUANTO")
                        {
                            actual.type = TokenType.WHILE;
                        }
                        else if (buffer.ToUpper() == "FIM_DO_LOOP")
                        {
                            actual.type = TokenType.WEND;
                        }
                        else if (buffer.ToUpper() == "SE")
                        {
                            actual.type = TokenType.IF;
                        }
                        else if (buffer.ToUpper() == "CONDICAO")
                        {
                            actual.type = TokenType.CONDICAO;
                        }
                        else if (buffer.ToUpper() == "ENTAO")
                        {
                            actual.type = TokenType.THEN;
                        }
                        else if (buffer.ToUpper() == "E")
                        {
                            actual.type = TokenType.AND;
                        }
                        else if (buffer.ToUpper() == "OU")
                        {
                            actual.type = TokenType.OR;
                        }
                        else if (buffer.ToUpper() == "ENTRADA")
                        {
                            actual.type = TokenType.INPUT;
                        }
                        else if (buffer.ToUpper() == "NAO")
                        {
                            actual.type = TokenType.NOT;
                        }
                        else if (buffer.ToUpper() == "CASO_CONTRARIO")
                        {
                            actual.type = TokenType.ELSE;
                        }
                        else if (buffer.ToUpper() == "CRIAR")
                        {
                            actual.type = TokenType.DIM;
                        }
                        else if (buffer.ToUpper() == "COMO")
                        {
                            actual.type = TokenType.AS;
                        }
                        else if (buffer.ToUpper() == "NUMERO")
                        {
                            actual.type = TokenType.INTEGER;
                        }
                        else if (buffer.ToUpper() == "BOOLEANO")
                        {
                            actual.type = TokenType.BOOLEAN;
                        }
                        else if (buffer.ToUpper() == "VERDADE")
                        {
                            actual.type = TokenType.BOOL;
                            actual.value = true;
                        }
                        else if (buffer.ToUpper() == "FALSO")
                        {
                            actual.type = TokenType.BOOL;
                            actual.value = false;
                        }
                        else if (buffer.ToUpper() == "CODIGO")
                        {
                            actual.type = TokenType.SUB;
                            actual.value = false;
                        }
                        else if (buffer.ToUpper() == "CHAMAR")
                        {
                            actual.type = TokenType.CALL;
                            actual.value = false;
                        }
                        else if (buffer.ToUpper() == "BLOCO")
                        {
                            actual.type = TokenType.FUNCTION;
                            actual.value = false;
                        }
                        else
                        {
                            actual.type = TokenType.IDENTIFIER;
                            actual.value = buffer;
                        }
                        position = iterator;
                        return actual;
                    }


                    if (buffer.Length > 0)
                    {
                        if (
                            origin[iterator] == '-' ||
                            origin[iterator] == '+' ||
                            origin[iterator] == '*' ||
                            origin[iterator] == '/' ||
                            origin[iterator] == '(' ||
                            origin[iterator] == ')' ||
                            origin[iterator] == '=' ||
                            origin[iterator] == '>' ||
                            origin[iterator] == '<' ||
                            origin[iterator] == ','
                            )
                        {
                            actual.type = TokenType.INT;
                            actual.value = Int32.Parse(buffer);
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

                    if (origin[iterator] == '+')
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

                    if (origin[iterator] == ',')
                    {
                        actual.type = TokenType.COMA;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == '=')
                    {
                        actual.type = TokenType.EQUAL;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == '>')
                    {
                        actual.type = TokenType.BIGGERTHEN;
                        position = iterator + 1;
                        return actual;
                    }

                    if (origin[iterator] == '<')
                    {
                        actual.type = TokenType.SMALLERTHEN;
                        position = iterator + 1;
                        return actual;
                    }


                    iterator++;

                }
                else
                {
                    if (buffer.Length > 0)
                    {
                        actual.type = TokenType.INT;
                        actual.value = Int32.Parse(buffer);
                        if ((origin[iterator] == '\n') || origin[iterator] == '\r')
                        {
                            position = iterator;
                        }
                        else
                        {
                            position = iterator + 1;
                        }
                        return actual;
                    }
                    else
                    {
                        if ((origin[iterator] == '\r'))
                        {
                            actual.type = TokenType.BREAKLINE;
                            position = iterator + 2; //because its  \r\n on windows
                            return actual;
                        }
                    }
                    iterator++;
                }
            }

            if (buffer.Length > 0)
            {
                actual.type = TokenType.INT;
                actual.value = Int32.Parse(buffer);
                position = iterator;
                return actual;
            }

            actual.type = TokenType.EOF;
            return actual;
        }

        public Token Spy(int tokensAhead = 1)
        {
            Token actualBackup = actual.Copy();
            int oldPosition = 0;
            while(oldPosition < position)
            {
                oldPosition += 1;
            }
            Token temp;
            int counter = 0;
            while(counter < tokensAhead - 1)
            {
                selectNext();
                counter += 1;
            }
            //got spy token
            temp = selectNext();

            //reseting values
            position = oldPosition;
            actual = actualBackup;

            //returning token
            return temp;
        }
    }
}

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
        
        public static Node parseExpression()
        {
            Node tree = parseTerm();
            while(
                tokens.actual.type == TokenType.PLUS || 
                tokens.actual.type == TokenType.MINUS || 
                tokens.actual.type == TokenType.OR)
            {
                tree = new BinOp(tokens.actual.Copy(), new List<Node>() { tree });
                tokens.selectNext();
                tree.children.Add(parseTerm());
            }
            return tree;
        }

        public static Node parseTerm()
        {
            Node tree = parseFactor();
            while (
                tokens.actual.type == TokenType.MULTIPLY || 
                tokens.actual.type == TokenType.DIVIDE || 
                tokens.actual.type == TokenType.AND)
            {
                tree = new BinOp(tokens.actual.Copy(), new List<Node>() { tree });
                tokens.selectNext();
                tree.children.Add(parseFactor());
            }
            return tree;
        }

        public static Node parseFactor()
        {
            Node output;
            if (tokens.actual.type == TokenType.INT)
            {
                output = new IntVal(tokens.actual.Copy());
                tokens.selectNext();
            } else if (tokens.actual.type == TokenType.IDENTIFIER) {
                output = new Identifier(tokens.actual.Copy());
                tokens.selectNext();
            } else if (tokens.actual.type == TokenType.PARENTHESESBEGIN)
            {
                tokens.selectNext();
                output = parseExpression();
                if (tokens.actual.type != TokenType.PARENTHESESEND)
                {
                    throw new Exception("Invalid Syntax - Missing )");
                }
                else
                {
                    tokens.selectNext();
                }
            } else if (
                tokens.actual.type == TokenType.MINUS || 
                tokens.actual.type == TokenType.PLUS || 
                tokens.actual.type == TokenType.NOT)
            {
                Token temp = tokens.actual.Copy();
                tokens.selectNext();
                output = new UnOp(temp, new List<Node>() { parseFactor() });
            } else if (tokens.actual.type == TokenType.INPUT)
            {
                output = new Input(tokens.actual.Copy());
                tokens.selectNext();
            } else if (tokens.actual.type == TokenType.BOOL)
            {
                output = new Bool(tokens.actual.Copy());
                tokens.selectNext();
            }
            else
            {
                throw new Exception("Unnespected Token");
            }
            return output;
        }


        public static Node parseType()
        {
            if(tokens.actual.type == TokenType.INTEGER || (tokens.actual.type == TokenType.BOOLEAN))
            {
                Node output = new Type(tokens.actual.Copy());
                tokens.selectNext();
                return output;
            } 
            throw new Exception("Invalid Syntax - Expecting Boolean or Integer token");
            
        }

        public static Node parseProgram()
        {
            Node tree = new Statements(new Token(), new List<Node>());
            if (tokens.actual.type == TokenType.SUB)
            {
                tokens.selectNext();
                if(tokens.actual.type == TokenType.MAIN)
                {
                    tokens.selectNext();
                    if(tokens.actual.type == TokenType.PARENTHESESBEGIN)
                    {
                        tokens.selectNext();
                        if(tokens.actual.type == TokenType.PARENTHESESEND)
                        {
                            tokens.selectNext();
                            if (tokens.actual.type != TokenType.BREAKLINE)
                            {
                                throw new Exception("Invalid Syntax -> Expecting new line");
                            }
                            tokens.selectNext();
                            while(tokens.actual.type != TokenType.END)
                            {
                                tree.children.Add(parseStatement());
                                if (tokens.actual.type != TokenType.BREAKLINE)
                                {
                                    throw new Exception("Invalid Syntax -> Expecting new line");
                                }
                                tokens.selectNext();
                            }
                            tokens.selectNext();
                            if (tokens.actual.type == TokenType.SUB)
                            {
                                tokens.selectNext();
                            }else
                            {
                                throw new Exception("Invalid Syntax -> Expecting SUB token");
                            }
                        } else
                        {
                            throw new Exception("Invalid Syntax -> Expecting ) after ( token");
                        }
                    } else
                    {
                        throw new Exception("Invalid Syntax -> Expecting ( after main token");
                    }
                } else
                {
                    throw new Exception("Invalid Syntax -> Expecting Main token after sub");
                }

            } else
            {
                throw new Exception("Invalid Syntax -> Expecting SUB token");
            }
            return tree;
        }

        public static  Node parseStatement()
        {
            Node output;
            if (tokens.actual.type == TokenType.IDENTIFIER)
            {
                Node identifier = new Identifier(tokens.actual.Copy());
                tokens.selectNext();
                output = new Assignment(tokens.actual.Copy(), new List<Node>() { identifier });
                tokens.selectNext();
                output.children.Add(parseExpression());
            } else if (tokens.actual.type == TokenType.PRINT)
            {
                output = new Print(tokens.actual.Copy(), new List<Node>());
                tokens.selectNext();
                output.children.Add(parseExpression());
            }
            else if (tokens.actual.type == TokenType.DIM)
            {
                output = new VarDec(tokens.actual.Copy(), new List<Node>());
                tokens.selectNext();
                if(tokens.actual.type == TokenType.IDENTIFIER)
                {
                    output.children.Add(new Identifier(tokens.actual.Copy()));
                    tokens.selectNext();
                    if (tokens.actual.type == TokenType.AS)
                    {
                        tokens.selectNext();
                        output.children.Add(parseType());
                    } else
                    {
                        throw new Exception("Invalid Syntax - Expecting As token");
                    }
                }
                else
                {
                    throw new Exception("Invalid Syntax - Expecting Identifier after DIM");
                }
            }
            else if (tokens.actual.type == TokenType.WHILE)
            {
                output = new While(tokens.actual.Copy(), new List<Node>());
                tokens.selectNext();
                output.children.Add(parseRelExpression());
                if (tokens.actual.type != TokenType.BREAKLINE)
                {
                    throw new Exception("Invalid Syntax -> Expecting new line");
                }
                tokens.selectNext();   
                Node whileStatements = new Statements(tokens.actual.Copy(), new List<Node>());
                while (tokens.actual.type != TokenType.WEND)
                {
                    whileStatements.children.Add(parseStatement());
                    if (tokens.actual.type != TokenType.BREAKLINE)
                    {
                        throw new Exception("Invalid Syntax -> Expecting new line");
                    }
                    tokens.selectNext();
                }
                output.children.Add(whileStatements);
                tokens.selectNext();
            } else if (tokens.actual.type == TokenType.IF) 
            {
                output = new If(tokens.actual.Copy(), new List<Node>());
                tokens.selectNext();
                Node firstSon = parseRelExpression();
                if(tokens.actual.type == TokenType.THEN)
                {
                    tokens.selectNext();
                    if(tokens.actual.type != TokenType.BREAKLINE)
                    {
                        throw new Exception("Invalid Syntax -> Expecting new line");
                    }
                    tokens.selectNext();
                    Node secondSon = new Statements(tokens.actual.Copy(), new List<Node>());
                    while ((tokens.actual.type != TokenType.ELSE) && (tokens.actual.type != TokenType.END))
                    {
                        secondSon.children.Add(parseStatement());
                        if (tokens.actual.type != TokenType.BREAKLINE)
                        {
                            throw new Exception("Invalid Syntax -> Expecting new line");
                        }
                        tokens.selectNext();
                    }
                    output.children.Add(firstSon);
                    output.children.Add(secondSon);
                }
                else
                {
                    throw new Exception("Invalid Syntax -> Missing THEN");
                }
                if(tokens.actual.type == TokenType.ELSE)
                {
                    tokens.selectNext();
                    if (tokens.actual.type != TokenType.BREAKLINE)
                    {
                        throw new Exception("Invalid Syntax -> Expecting new line");
                    }
                    tokens.selectNext();
                    Node thirdSon = new Statements(tokens.actual.Copy(), new List<Node>());
                    while (tokens.actual.type != TokenType.END)
                    {
                        thirdSon.children.Add(parseStatement());
                        if (tokens.actual.type != TokenType.BREAKLINE)
                        {
                            throw new Exception("Invalid Syntax -> Expecting new line");
                        }
                        tokens.selectNext();
                    }
                    output.children.Add(thirdSon);
                    tokens.selectNext();
                    if (tokens.actual.type == TokenType.IF)
                    {
                        tokens.selectNext();
                    }
                    else
                    {
                        throw new Exception("Invalid Syntax -> Missing IF of END IF");
                    }
                }
                else if (tokens.actual.type == TokenType.END)
                {
                    tokens.selectNext();
                    if (tokens.actual.type == TokenType.IF)
                    {
                        tokens.selectNext();
                    }
                    else
                    {
                        throw new Exception("Invalid Syntax -> Missing IF of END IF");
                    }
                }
                else
                {
                    throw new Exception("Invalid Syntax -> Missing END IF");
                }
            }
            else
            {
                output = new NoOp();
            }
            return output;
        }

        public static Node parseRelExpression()
        {
            Node output; 
            Node firstSon = parseExpression();
            if (
                (tokens.actual.type == TokenType.EQUAL) ||
                (tokens.actual.type == TokenType.BIGGERTHEN) || 
                (tokens.actual.type == TokenType.SMALLERTHEN)
                )
            {
               output =  new BinOp(tokens.actual.Copy(), new List<Node>() { firstSon });
               tokens.selectNext();
               output.children.Add(parseExpression());
                return output;
            } else
            {
                throw new Exception("Essa condição não é válida");
            }    
        }

        public static Node run(string input)
        {
            Parser.tokens = new Tokenizer() { origin = input, position = 0, actual = new Token() };
            tokens.selectNext();
            Node tree = parseProgram();
            if (tokens.actual.type != TokenType.EOF)
            {
                throw new Exception("Entrada inválida, a cadeia terminou sem um END");
            }
            return tree;
        }
    }
}

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
            }
            else
            {
                throw new Exception("Unnespected Token");
            }
            return output;
        }

        public static Node parseStatements()
        {
            Node tree = new Statements(new Token(), new List<Node>());
            tree.children.Add(parseStatement());
            while (tokens.actual.type == TokenType.BREAKLINE)
            {
                tokens.selectNext();
                tree.children.Add(parseStatement());
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
                output.children.Add(parseStatements());
                if(tokens.actual.type != TokenType.WEND)
                {
                    throw new Exception("Invalid Syntax -> Missing WEND");
                } else
                {
                    tokens.selectNext();
                }
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
                    Node secondSon = parseStatements();
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
                    output.children.Add(parseStatements());
                    if(tokens.actual.type == TokenType.END)
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
            Node tree = parseStatements();
            if (tokens.actual.type != TokenType.EOF)
            {
                throw new Exception("Entrada inválida, a cadeia terminou sem um END");
            }
            return tree;
        }
    }
}

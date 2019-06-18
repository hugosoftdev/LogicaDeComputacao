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
                if (tokens.Spy().type != TokenType.PARENTHESESBEGIN)
                {
                    output = new Identifier(tokens.actual.Copy());
                    tokens.selectNext();
                } else
                {
                    Token funcIdentifier = tokens.actual.Copy();
                    tokens.selectNext();
                    tokens.selectNext();
                    List<Node> arguments = new List<Node>();
                    if (tokens.actual.type == TokenType.PARENTHESESEND)
                    {
                        tokens.selectNext();
                    }
                    else
                    {
                        arguments.Add(parseRelExpression());
                        while (tokens.actual.type == TokenType.COMA)
                        {
                            tokens.selectNext();
                            arguments.Add(parseRelExpression());
                        }
                        if (tokens.actual.type != TokenType.PARENTHESESEND)
                        {
                            throw new Exception("Exepecting ) to finish func call");
                        }
                        tokens.selectNext();
                    }
                    output = new FuncCall(funcIdentifier, arguments);
                }               
            } else if (tokens.actual.type == TokenType.PARENTHESESBEGIN)
            {
                tokens.selectNext();
                output = parseRelExpression();
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
            while ((tokens.actual.type == TokenType.SUB) || (tokens.actual.type == TokenType.FUNCTION)) {
                if (tokens.actual.type == TokenType.SUB)
                {
                    tree.children.Add(parseSubDec());
                }
                else
                {
                    tree.children.Add(parseFuncDec());
                }
            }
            return tree;
        }

        public static Node parseSubDec()
        {
            tokens.selectNext();
            Node tree = new SubDec(tokens.actual.Copy(), new List<Node>());
            if (tokens.actual.type != TokenType.IDENTIFIER)
            {
                throw new Exception("Expecting Identifier after sub token");
            }
            tokens.selectNext();
            if(tokens.actual.type != TokenType.PARENTHESESBEGIN)
            {
                throw new Exception("Expecting ( token");
            }
            tokens.selectNext();
            if ((tokens.actual.type != TokenType.PARENTHESESEND) && 
                (tokens.actual.type != TokenType.IDENTIFIER))
            {
                throw new Exception("Expecting ) or variable declaration");
            }

            if (tokens.actual.type == TokenType.IDENTIFIER)
            {
                Node vardec;

                vardec = new VarDec(tokens.actual.Copy(), new List<Node>());
                vardec.children.Add(new Identifier(tokens.actual.Copy()));
                tokens.selectNext();
                if (tokens.actual.type != TokenType.AS)
                {
                    throw new Exception("Expecting AS after identifier declaration");
                }
                tokens.selectNext();
                vardec.children.Add(parseType());
                tree.children.Add(vardec);

                while (tokens.actual.type == TokenType.COMA)
                {
                    tokens.selectNext();
                    vardec = new VarDec(tokens.actual.Copy(), new List<Node>());
                    vardec.children.Add(new Identifier(tokens.actual.Copy()));
                    tokens.selectNext();
                    if (tokens.actual.type != TokenType.AS)
                    {
                        throw new Exception("Expecting AS after identifier declaration");
                    }
                    tokens.selectNext();
                    vardec.children.Add(parseType());
                    tree.children.Add(vardec);
                }
            }

            tokens.selectNext(); //skipping )

            if(tokens.actual.type != TokenType.BREAKLINE)
            {
                throw new Exception("Expecting new Line");
            }
            tokens.selectNext();

            Node statements = new Statements(new Token(), new List<Node>());
            if (tokens.Spy().type == TokenType.END)
            {
                tokens.selectNext();
                if(tokens.actual.type != TokenType.SUB)
                {
                    throw new Exception("Expecting SUB after END");
                }
                tokens.selectNext();
                tree.children.Add(statements);
                return tree;
            }

        
            statements.children.Add(parseStatement());
            if(tokens.actual.type != TokenType.BREAKLINE)
            {
                throw new Exception("Expecting breakline");
            }
            tokens.selectNext();

            while(tokens.actual.type != TokenType.END)
            {
                statements.children.Add(parseStatement());
                if (tokens.actual.type != TokenType.BREAKLINE)
                {
                    throw new Exception("Expecting breakline");
                }
                tokens.selectNext();
            }

            tokens.selectNext();
            if(tokens.actual.type != TokenType.SUB)
            {
                throw new Exception("Expecting SUB after END TOKEN");
            }
            tokens.selectNext();
            tree.children.Add(statements);
            if((tokens.actual.type != TokenType.BREAKLINE) && (tokens.actual.type != TokenType.EOF))
            {
                throw new Exception("Expecting breakline");
            }
            tokens.selectNext();
            return tree;
        }

        public static Node parseFuncDec()
        {            
            tokens.selectNext();
            if (tokens.actual.type != TokenType.IDENTIFIER)
            {
                throw new Exception("Expecting Identifier after function token");
            }
            Token actualToken = tokens.actual.Copy();
            tokens.selectNext();

            if (tokens.actual.type != TokenType.PARENTHESESBEGIN)
            {
                throw new Exception("Expecting ( token");
            }
            tokens.selectNext();
            if ((tokens.actual.type != TokenType.PARENTHESESEND) &&
                (tokens.actual.type != TokenType.IDENTIFIER))
            {
                throw new Exception("Expecting ) or variable declaration");
            }

            List<Node> varDecs = new List<Node>();

            if (tokens.actual.type == TokenType.IDENTIFIER)
            {
                Node vardec;

                vardec = new VarDec(tokens.actual.Copy(), new List<Node>());
                vardec.children.Add(new Identifier(tokens.actual.Copy()));
                tokens.selectNext();
                if (tokens.actual.type != TokenType.AS)
                {
                    throw new Exception("Expecting AS after identifier declaration");
                }
                tokens.selectNext();
                vardec.children.Add(parseType());
                varDecs.Add(vardec);

                while (tokens.actual.type == TokenType.COMA)
                {
                    tokens.selectNext();
                    vardec = new VarDec(tokens.actual.Copy(), new List<Node>());
                    vardec.children.Add(new Identifier(tokens.actual.Copy()));
                    tokens.selectNext();
                    if (tokens.actual.type != TokenType.AS)
                    {
                        throw new Exception("Expecting AS after identifier declaration");
                    }
                    tokens.selectNext();
                    vardec.children.Add(parseType());
                    varDecs.Add(vardec);
                }
            }

            tokens.selectNext(); //skipping )

            if (tokens.actual.type != TokenType.AS)
            {
                throw new Exception("Expecting AS after identifier of func");
            }
            tokens.selectNext();

            Node tree = new FuncDec(actualToken, varDecs, parseType());

            if (tokens.actual.type != TokenType.BREAKLINE)
            {
                throw new Exception("Expecting new Line");
            }

            tokens.selectNext();

            Node statements = new Statements(new Token(), new List<Node>());

            if (tokens.Spy().type == TokenType.END)
            {
                tokens.selectNext();
                if (tokens.actual.type != TokenType.FUNCTION)
                {
                    throw new Exception("Expecting FUNCTION after END");
                }
                tokens.selectNext();
                tree.children.Add(statements);
                return tree;
            }

            statements.children.Add(parseStatement());
            if (tokens.actual.type != TokenType.BREAKLINE)
            {
                throw new Exception("Expecting breakline");
            }
            tokens.selectNext();

            while (tokens.actual.type != TokenType.END)
            {
                statements.children.Add(parseStatement());
                if (tokens.actual.type != TokenType.BREAKLINE)
                {
                    throw new Exception("Expecting breakline");
                }
                tokens.selectNext();
            }

            tokens.selectNext();
            if (tokens.actual.type != TokenType.FUNCTION)
            {
                throw new Exception("Expecting FUNCTION after END TOKEN");
            }
            tokens.selectNext();
            tree.children.Add(statements);
            if ((tokens.actual.type != TokenType.BREAKLINE) && (tokens.actual.type != TokenType.EOF))
            {
                throw new Exception("Expecting breakline");
            }
            tokens.selectNext();
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
            } else if (tokens.actual.type == TokenType.CALL)
            {
                tokens.selectNext();
                if(tokens.actual.type != TokenType.IDENTIFIER)
                {
                    throw new Exception("Expecting sub identifier after call token");
                }
                Token funcIdentifier = tokens.actual.Copy();
                tokens.selectNext();

                if (tokens.actual.type != TokenType.PARENTHESESBEGIN)
                {
                    throw new Exception("Expecting ( after sub identifier");
                }
                tokens.selectNext();
                List<Node> arguments = new List<Node>();
                if(tokens.actual.type == TokenType.PARENTHESESEND) {
                    tokens.selectNext();
                } else
                {
                    arguments.Add(parseExpression());
                    while(tokens.actual.type == TokenType.COMA)
                    {
                        tokens.selectNext();
                        arguments.Add(parseExpression());
                    }
                    if(tokens.actual.type != TokenType.PARENTHESESEND)
                    {
                        throw new Exception("Exepecting ) to finish sub call");
                    }
                    tokens.selectNext();
                }
                output = new SubCall(funcIdentifier, arguments);
            }
            else if (tokens.actual.type == TokenType.PRINT)
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
            Node output = parseExpression();
            if (
                (tokens.actual.type == TokenType.EQUAL) ||
                (tokens.actual.type == TokenType.BIGGERTHEN) || 
                (tokens.actual.type == TokenType.SMALLERTHEN)
                )
            {
               output =  new BinOp(tokens.actual.Copy(), new List<Node>() { output });
               tokens.selectNext();
               output.children.Add(parseExpression());
            }
            return output;
        }

        public static Node run(string input)
        {
            Parser.tokens = new Tokenizer() { origin = input, position = 0, actual = new Token() };
            tokens.selectNext();
            Node tree = parseProgram();
            tree.children.Add(new SubCall(new Token()
            { value = "main", type = TokenType.IDENTIFIER }, new List<Node>()));
            if (tokens.actual.type != TokenType.EOF)
            {
                throw new Exception("Entrada inválida, a cadeia terminou sem um EOF");
            }
            return tree;
        }
    }
}

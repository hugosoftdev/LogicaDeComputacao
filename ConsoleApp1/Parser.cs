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
            while(tokens.actual.type == TokenType.PLUS || tokens.actual.type == TokenType.MINUS)
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
            while (tokens.actual.type == TokenType.MULTIPLY || tokens.actual.type == TokenType.DIVIDE)
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
            } else if (tokens.actual.type == TokenType.MINUS || tokens.actual.type == TokenType.PLUS)
            {
                Token temp = tokens.actual.Copy();
                tokens.selectNext();
                output = new UnOp(temp, new List<Node>() { parseFactor() });
            } else
            {
                throw new Exception("Unnespected Token");
            }
            return output;
        }

        public static Node parseStatements()
        {
            if(tokens.actual.type == TokenType.BEGIN)
            {
                //pular linha
                if(tokens.origin[tokens.position+1].Equals('\n'))
                {
                    tokens.position += 2;
                }
                tokens.selectNext();
                Node tree = new Statements(new Token(), new List<Node>());
                while(tokens.actual.type != TokenType.END)
                {
                    tree.children.Add(parseStatement());
                }
                tokens.selectNext();
                return tree;
            } else
            {
                throw new Exception("Missing BEGIN token");
            }
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
            } else if(tokens.actual.type == TokenType.BEGIN)
            {
                output = parseStatements();
            } else
            {
                output = new NoOp();
            }
            return output;
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

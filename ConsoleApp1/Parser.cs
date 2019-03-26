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
            if(tokens.actual.type == TokenType.INT)
            {
                output = new IntVal(tokens.actual.Copy());
                tokens.selectNext();
            }  else if(tokens.actual.type == TokenType.PARENTHESESBEGIN)
            {
                tokens.selectNext();
                output = parseExpression();
                if(tokens.actual.type != TokenType.PARENTHESESEND)
                {
                    throw new Exception("Invalid Syntax - Missing )");
                }
                else
                {
                    tokens.selectNext();
                }
            } else if(tokens.actual.type == TokenType.MINUS || tokens.actual.type == TokenType.PLUS)
            {
                Token temp = tokens.actual.Copy();
                tokens.selectNext();
                output = new UnOp(temp, new List<Node>() { parseFactor() });
            }else
            {
                throw new Exception("Unnespected Token");
            }
            return output;
        }

        public static Node run(string input)
        {
            Parser.tokens = new Tokenizer() { origin = input, position = 0, actual = new Token() };
            tokens.selectNext();
            Node tree = parseExpression();
            if (tokens.actual.type != TokenType.EOF)
            {
                throw new Exception("Entrada inválida, a cadeia terminou sem um EOF");
            }
            return tree;
        }
    }
}

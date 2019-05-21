using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class BinOp : Node
    {
        public BinOp(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            EvaluateReturn firstNumber = this.children.ElementAt(0).Evaluate(st);
            NasmManager.AddLine("PUSH EBX");
            EvaluateReturn secondNumber = this.children.ElementAt(1).Evaluate(st);

            if ((TokenType) firstNumber.type != (TokenType) secondNumber.type)
            {
                throw new Exception("Operação com dois tipos diferentes de variável");
            }

            Token token = (Token) this.value;


            NasmManager.AddLine("POP EAX");

            if (token.type == TokenType.MINUS && ((TokenType) firstNumber.type == TokenType.INT))
            {
                NasmManager.AddLine("SUB EAX, EBX");
                NasmManager.AddLine("MOV EBX, EAX");
                return new EvaluateReturn() { value= (int) firstNumber.value - (int) secondNumber.value, type = TokenType.INT };
            } else if(token.type == TokenType.PLUS && ((TokenType)firstNumber.type == TokenType.INT))
            {
                NasmManager.AddLine("ADD EAX, EBX");
                NasmManager.AddLine("MOV EBX, EAX");
                return new EvaluateReturn() { value = (int)firstNumber.value + (int)secondNumber.value, type = TokenType.INT };
            } else if(token.type == TokenType.MULTIPLY && ((TokenType)firstNumber.type == TokenType.INT))
            {
                NasmManager.AddLine("IMUL EBX");
                NasmManager.AddLine("MOV EBX, EAX");
                return new EvaluateReturn() { value = (int)firstNumber.value * (int)secondNumber.value, type = TokenType.INT };
            } else if(token.type == TokenType.DIVIDE && ((TokenType)firstNumber.type == TokenType.INT))
            {
                NasmManager.AddLine("IDIV EBX");
                NasmManager.AddLine("MOV EBX, EAX");
                return new EvaluateReturn() { value = (int)firstNumber.value / (int)secondNumber.value, type = TokenType.INT };
            } else if (token.type == TokenType.BIGGERTHEN && ((TokenType)firstNumber.type == TokenType.BOOL))
            {
                NasmManager.AddLine("CMP EAX, EBX");
                NasmManager.AddLine("CALL binop_jg");
                return new EvaluateReturn() { value = (int)firstNumber.value > (int)secondNumber.value, type = TokenType.INT };
            }
            else if (token.type == TokenType.SMALLERTHEN && ((TokenType)firstNumber.type == TokenType.INT))
            {
                NasmManager.AddLine("CMP EAX, EBX");
                NasmManager.AddLine("CALL binop_jl");
                return new EvaluateReturn() { value = (int) firstNumber.value < (int)secondNumber.value, type = TokenType.BOOL };
            }
            else if (token.type == TokenType.AND && ((TokenType)firstNumber.type == TokenType.BOOL))
            {
                NasmManager.AddLine("AND EAX, EBX");
                NasmManager.AddLine("MOV EBX, EAX");
                return new EvaluateReturn() { value = (bool)firstNumber.value && (bool)secondNumber.value, type = TokenType.BOOL };
            }
            else if (token.type == TokenType.OR && ((TokenType)firstNumber.type == TokenType.BOOL))
            {
                NasmManager.AddLine("OR EAX, EBX");
                NasmManager.AddLine("MOV EBX, EAX");
                return new EvaluateReturn() { value = (bool)firstNumber.value || (bool)secondNumber.value, type = TokenType.BOOL };
            }
            else if (token.type == TokenType.EQUAL)
            {
                NasmManager.AddLine("CMP EAX, EBX");
                NasmManager.AddLine("CALL binop_je");
                if ((TokenType) firstNumber.type == TokenType.INT)
                {
                    return new EvaluateReturn() { value = (int) firstNumber.value == (int) secondNumber.value, type = TokenType.BOOL };
                } else
                {
                    return new EvaluateReturn() { value = (bool) firstNumber.value == (bool) secondNumber.value, type = TokenType.BOOL };
                }
            } else
            {
                throw new Exception("Tipo de variável não compatível com a operação realizada");
            }
            throw new Exception("Um Nó foi classificado como BinOp sem ser um token do tipo PLUS | MINUS | MULTIPLY | DIVIDE || SMALLERTHEN || BIGGERTHEN || EQUAL");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SubCall : Node
    {
        public SubCall(Token value, List<Node> children)
        {
            this.value = value;
            this.children = children;
        }

        override public EvaluateReturn Evaluate(SymbolTable st)
        {
            //criando nova symbol table para escopo
            SymbolTable newScope = new SymbolTable();
            newScope.Daddy = st;

            //resgatando função da st   
            Token token = (Token)value;
            string key = (string)token.value;
            Node function;
            function = (Node) st.GetValue(key, true);

            //checking if arguments passed has the same length
            int n_args = function.children.Count() - 1;
            if (children.Count() != (n_args))
            {
                throw new Exception($"Func {key} receives {n_args} but {children.Count()} were given");
            }
            int counter = 0;


            //inicializacao das variaveis no novo scopo
            while (counter < n_args )
            {

                //inicializa vardecs
                function.children.ElementAt(counter).Evaluate(newScope);

                //seta valor das variaveis com base nos argumentos passados
                EvaluateReturn arg = children.ElementAt(counter).Evaluate(st);
                Token variableToken = (Token) function.children.ElementAt(counter).children.ElementAt(0).value;
                string declarationVariableName = (string) variableToken.value;
                TokenType stType = (TokenType) newScope.GetType(declarationVariableName);
                if (stType == TokenType.INTEGER)
                {
                    if((TokenType) arg.type != TokenType.INT)
                    {
                        throw new Exception("Cant convert to int");
                    }
                    newScope.SetValue(declarationVariableName, (int) arg.value);
                }
                else if (stType == TokenType.BOOLEAN)
                {
                    if ((TokenType)arg.type != TokenType.BOOL)
                    {
                        throw new Exception("Cant convert to bool");
                    }
                    newScope.SetValue(declarationVariableName, (bool) arg.value);
                }
                counter += 1;
            }

            //running function code
            int statementsIndex = n_args;
            function.children.ElementAt(statementsIndex).Evaluate(newScope);
            
            return new EvaluateReturn() { value = null, type = null };

        }
    }
}

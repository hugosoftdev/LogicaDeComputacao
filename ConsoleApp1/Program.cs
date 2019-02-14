using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator();
        }

        static void CheckInvalidFormat(string text)
        {
            if(text.Length == 0)
            {
                Console.WriteLine("Texto de tamanho 0");
                throw new ArgumentException();
            }


            if(text[0] == '+' || (text[0] == '-'))
            {
                Console.WriteLine("Não é permitido começar uma conta com sinal");
                throw new ArgumentException();
            }
        }

        static void Calculator()
        {
            Console.WriteLine("Insira a conta: ");
            string input = Console.ReadLine();
            try
            {
                input = input.Replace("  ", string.Empty);
                List<int> operations = new List<int>();

                CheckInvalidFormat(input);
                operations.Add(1); //para ja começar somando o primeiro número

                foreach (char c in input)
                {
                    if (c == '+')
                    {
                        operations.Add(1);
                    }
                    else if (c == '-')
                    {
                        operations.Add(0);
                    }
                }

                input = input.Replace("+", " ");
                input = input.Replace("-", " ");
                string[] words = input.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                if(words.Length < 2)
                {
                    Console.WriteLine("Uma conta precisa de 2 números ou mais para realizar a operação");
                    throw new ArgumentException();
                }

                if(operations.Count > words.Length)
                {
                    Console.WriteLine("Quantidade inválida de operadores (+ , -)");
                    throw new ArgumentException();
                }

                int output = 0;
                for (int counter = 0; counter < words.Length; counter++)
                {
                    int numero = int.Parse(words[counter]);
                    if (operations[counter] == 1)
                    {
                        output = output + int.Parse(words[counter]);
                    }
                    else
                    {
                        output = output - int.Parse(words[counter]);
                    }
                }
                Console.WriteLine(output);
            }
            catch (Exception E)
            {
                Console.WriteLine("Erro, insira um formato válido");
            }

            Console.WriteLine("\n \n");
            Calculator();
        }
    }
}

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
            MainLoop();
        }

        static void MainLoop()
        {
            Console.WriteLine("Insira a expressão: ");
            string input = Console.ReadLine();
            input = PrePro.filter(input);
            Console.WriteLine("\n");
            try
            {
                Console.WriteLine("Resultado: " + Parser.run(input));
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\n");
            MainLoop();
        }
    }
}

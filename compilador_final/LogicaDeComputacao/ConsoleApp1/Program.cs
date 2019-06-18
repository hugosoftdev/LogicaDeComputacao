using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MainProgram();
        }

        static void MainProgram()
        {
            string path = "./input.vbs";
            string input = File.ReadAllText(path);
            input = PrePro.filter(input);
            Console.WriteLine("Pre processed Code: \n");
            Console.WriteLine(input);
            Console.WriteLine("\n");
            try
            {
                SymbolTable st = new SymbolTable();
                Parser.run(input).Evaluate(st);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\n");
            System.Threading.Thread.Sleep(10000);
        }
    }
}

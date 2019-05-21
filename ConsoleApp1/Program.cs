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
            Console.WriteLine(input);
            input = PrePro.filter(input);
            Console.WriteLine("\n");
            try
            {
                SymbolTable st = new SymbolTable();
                NasmManager.InitFile();
                Parser.run(input).Evaluate(st);
                NasmManager.EndFile();
                CreateFile();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\n");
            Console.WriteLine(NasmManager.Code);
            System.Threading.Thread.Sleep(100000000);
        }

        static void CreateFile()
        {
            string path = "./output.nasm";
            File.WriteAllText(path, NasmManager.Code);
        }
    }
}

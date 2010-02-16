using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tccconsole
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsoleColor.SetForeGroundColour(ConsoleColor.ForeGroundColour.Green, true);
            Console.WriteLine("parCer : A Lexer/Parser for C (Aivan Monceller && Patrick Cardenas)");
            ConsoleColor.SetForeGroundColour();
            Console.Write("Please input filename: ");
            string filename = Console.ReadLine();
            if (String.IsNullOrEmpty(filename))
                Environment.Exit(1);
            TestFromFile(filename);
            //TestFromFile("input.txt");
            Console.ReadKey(true);

        }

        public static void TestFromFile(string filename)
        {
            StreamReader reader = null;

            reader = new StreamReader(filename);

            Scanner scanner = new Scanner();

            Parser parser = new Parser(scanner);

            string input = reader.ReadToEnd();


            ParseTree tree = parser.Parse(input);
            int a = 0;
            foreach (ParseError err in tree.Errors)
            {
                ConsoleColor.SetForeGroundColour(ConsoleColor.ForeGroundColour.Red, true);
                Console.WriteLine("Line: {0,3}, Column: {1,3} : {2}",err.Line,err.Column,err.Message);
                ConsoleColor.SetForeGroundColour();
                a++;
                break;
            }
            if (a > 0)
                Console.WriteLine("{0} Error Found.", a);
            else
                Console.WriteLine("No Errors Found.");

        }

    }
}

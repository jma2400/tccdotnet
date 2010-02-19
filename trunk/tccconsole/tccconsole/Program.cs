﻿using System;
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
            StreamWriter fstream = new StreamWriter("tokens.txt", false);

            StreamReader reader = null;
            reader = new StreamReader(filename);

            Scanner scanner = new Scanner();

            string input = reader.ReadToEnd();

            scanner.Init(input);
            Token tok = new Token();
            do
            {
                //tok = scanner.Scan(scanner.Tokens.ToArray());
                tok = scanner.Scan();
                //Here yay
                fstream.WriteLine("Token: {0} \r\n Type: {1,-12} Line: {2,3} \r\n", tok.Text, tok.Type, tok.LinePos);
            } while (input.Length != tok.EndPos);

            
            //Token tkn = new Token();
            //do
            //{
            //    tkn = scanner.LookAhead();
            //}
            //while (true);

        }

    }
}
using System;

namespace Packlet.Core
{
    public class Screen
    {
        public static void Print(string text, ConsoleColor color = ConsoleColor.White) 
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintLn(string text, ConsoleColor color = ConsoleColor.White) 
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Clear() => Console.Clear();
    }
}
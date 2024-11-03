using SavannaApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services
{
    public class GameConsole : IGameConsole
    {
        public Encoding OutputEncoding { get; set; } = System.Text.Encoding.UTF8;

        public bool KeyAvailable => Console.KeyAvailable;

        public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
        public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }

        public GameConsole(TextWriter? textWriter = null, TextReader? textReader = null)
        {
            if (textWriter != null)
            {
                Console.SetOut(textWriter);
            }

            if (textReader != null)
            {
                Console.SetIn(textReader);
            }

            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void ReadKey()
        {
            Console.ReadKey();
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public void Write(string? value)
        {
            Console.Write(value);
        }

        public void WriteLine(string? value)
        {
            Console.WriteLine(value);
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}

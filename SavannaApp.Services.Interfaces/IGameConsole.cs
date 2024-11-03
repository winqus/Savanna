using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Interfaces
{
    public interface IGameConsole
    {
        Encoding OutputEncoding { get; set; }

        bool KeyAvailable { get; }

        ConsoleColor ForegroundColor { get; set; }

        ConsoleColor BackgroundColor { get; set; }

        void Clear();

        void WriteLine(string? value);

        void Write(string? value);

        string? ReadLine();

        void ReadKey();

        ConsoleKeyInfo ReadKey(bool intercept);

        void ResetColor();


    }
}

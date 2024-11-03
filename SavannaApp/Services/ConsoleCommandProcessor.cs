using SavannaApp.Interfaces;
using SavannaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services
{
    public class ConsoleCommandProcessor : ICommandProcessor
    {
        private readonly IGameConsole _console;

        private ConsoleKey? _key;

        public ConsoleCommandProcessor(IGameConsole console)
        {
            _console = console;
            _key = null;
        }

        public Command Command => _key switch
        {
            ConsoleKey.Escape => Command.Esc,
            ConsoleKey.D0 => Command.D0,
            ConsoleKey.D1 => Command.D1,
            ConsoleKey.D2 => Command.D2,
            ConsoleKey.D3 => Command.D3,
            ConsoleKey.D4 => Command.D4,
            ConsoleKey.D5 => Command.D5,
            ConsoleKey.D6 => Command.D6,
            ConsoleKey.D7 => Command.D7,
            ConsoleKey.D8 => Command.D8,
            ConsoleKey.D9 => Command.D9,
            ConsoleKey.A => Command.A,
            ConsoleKey.B => Command.B,
            ConsoleKey.C => Command.C,
            ConsoleKey.D => Command.D,
            ConsoleKey.E => Command.E,
            ConsoleKey.F => Command.F,
            ConsoleKey.G => Command.G,
            ConsoleKey.H => Command.H,
            ConsoleKey.I => Command.I,
            ConsoleKey.J => Command.J,
            ConsoleKey.K => Command.K,
            ConsoleKey.L => Command.L,
            ConsoleKey.M => Command.M,
            ConsoleKey.N => Command.N,
            ConsoleKey.O => Command.O,
            ConsoleKey.P => Command.P,
            ConsoleKey.Q => Command.Q,
            ConsoleKey.R => Command.R,
            ConsoleKey.S => Command.S,
            ConsoleKey.T => Command.T,
            ConsoleKey.U => Command.U,
            ConsoleKey.V => Command.V,
            ConsoleKey.W => Command.W,
            ConsoleKey.X => Command.X,
            ConsoleKey.Y => Command.Y,
            ConsoleKey.Z => Command.Z,
            _ => Command.NONE
        };

        public void Process()
        {
            try
            {
                if (_console.KeyAvailable)
                {
                    _key = _console.ReadKey(true).Key;

                    ClearInputBuffer();
                }
                else
                {
                    _key = null;
                }
            }
            catch (IOException ex)
            {
                // Thrown when after key press suddenly closing the application.
                return;
            }
        }

        public void ClearInputBuffer()
        {
            while (_console.KeyAvailable)
            {
                _console.ReadKey(true);
            }
        }

        public void LogKey()
        {
            if (_key != null)
            {
                _console.WriteLine($"Key-{_key} available.");
            }
        }
    }
}

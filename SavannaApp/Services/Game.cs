using SavannaApp.Animals;
using SavannaApp.Interfaces;
using SavannaApp.Models;
using SavannaApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services
{
    public class Game : IGame
    {
        private readonly IGameConsole _console;
        private readonly IAnimalField _field;
        private readonly IVisualizer _visualizer;
        private readonly ICommandProcessor _commandProcessor;
        private readonly IBreeder _breeder;
        private readonly IAnimalLoader _animalLoader;

        public Game(IGameConsole console, IAnimalField animalField, IVisualizer visualizer, ICommandProcessor commandProcessor, IBreeder breeder, IAnimalLoader animalLoader)
        {
            _console = console;
            _field = animalField;
            _visualizer = visualizer;
            _commandProcessor = commandProcessor;
            _breeder = breeder;
            _animalLoader = animalLoader;
        }

        public void StartGame()
        {
            //_field.InitializeEmptyField(10, 7);
            //_field.InitializeEmptyField(30, 40);
            _field.InitializeEmptyField(5, 5);
            _breeder.Initialize(1);
        }

        public void RunGame()
        {
            var availableSpawningCommands = _animalLoader.GetAnimalTypes().Select(x => (Command)Enum.Parse(typeof(Command), x.Name.Substring(0, 1)));

            Parallel.Invoke(() =>
                {
                    while (_commandProcessor.Command != Command.Esc)
                    {
                        lock (this)
                        {
                            _visualizer.Visualize(_field.FieldData);
                            _field.Update();
                            _breeder.Update();
                        }
                        _console.WriteLine($"Updates: {_field.FieldData.Updates}");
                        _console.WriteLine($"Press ESC to stop.");
                        Thread.Sleep(1000);
                    }
                },
                () =>
                {
                    while (_commandProcessor.Command != Command.Esc)
                    {
                        lock (this)
                        {
                            _commandProcessor.Process();

                            if(availableSpawningCommands.Contains(_commandProcessor.Command))
                            {
                                var newAnimal = Activator.CreateInstance(
                                    _animalLoader.GetAnimalTypes()
                                        .First(x => x.Name.StartsWith(_commandProcessor.Command.ToString()))
                                ) as Animal;

                                _field.AddAnimal(newAnimal);
                            }
                        }
                        Thread.Sleep(20);
                    }
                });
        }

    }
}

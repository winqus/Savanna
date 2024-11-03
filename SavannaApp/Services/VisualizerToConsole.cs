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
    public class VisualizerToConsole : IVisualizer
    {
        private readonly IGameConsole _console;
        private readonly IAnimalLoader _animalLoader;

        public string CharacterForLion { get; set; } = "L";

        public string CharacterForAntelope { get; set; } = "A";

        public string CharacterForEmpty { get; set; } = "#";

        public ConsoleColor AnimalAboutToDieColor { get; set; } = ConsoleColor.DarkYellow;

        public ConsoleColor AnimalAtMaxHealthColor { get; set; } = ConsoleColor.Green;

        public VisualizerToConsole(IGameConsole console, IAnimalLoader animalLoader)
        {
            _console = console;
            _animalLoader = animalLoader;
        }

        public void Visualize(AnimalFieldModel animalFieldModel)
        {
            _console.Clear();
            _console.WriteLine($"Field size: {animalFieldModel.FieldLength} x {animalFieldModel.FieldHeight}");

            for (int row = 0; row < animalFieldModel.FieldHeight; row++)
            {
                for (int col = 0; col < animalFieldModel.FieldLength; col++)
                {
                    var animal = animalFieldModel.Animals[row, col];

                    string outputChar = CharacterForEmpty;
                    
                    if(animal != null)
                    {
                        if(_animalLoader.HasType(animal.GetType()))
                        {
                            outputChar = animal.GetType().Name.Substring(0, 1);
                        }

                        if (animal.Data.Health <= 1)
                        {
                            _console.ForegroundColor = AnimalAboutToDieColor;
                        }

                        if(animal.Data.Health <= 1)
                        {
                            _console.ForegroundColor = AnimalAboutToDieColor;
                        }
                        else if(animal.Data.Health == animal.Data.MaxHealth)
                        {
                            _console.ForegroundColor = AnimalAtMaxHealthColor;
                        }
                    }
                    
                    _console.Write(outputChar);
                    _console.ResetColor();
                }

                _console.WriteLine("");
            }

            _console.WriteLine("Press a key to spawn an animal:");
            foreach (var type in _animalLoader.GetAnimalTypes())
            {
                _console.WriteLine($"[{type.Name[0]}] {type.Name}");
            }
        }
    }
}

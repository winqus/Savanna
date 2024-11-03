using SavannaApp.Models;
using SavannaApp.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Interfaces
{
    public interface IAnimalField
    {
        AnimalFieldModel FieldData { get; }

        void InitializeEmptyField(int length, int height);

        void Update();

        Animal? AddAnimal(Animal newAnimal);

        Animal? AddAnimal(Animal newAnimal, int row, int col);
    }
}

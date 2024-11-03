using SavannaApp.Animals;
using SavannaApp.Interfaces;
using SavannaApp.Models;
using SavannaApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services
{
    public class AnimalField : IAnimalField
    {
        private AnimalFieldModel _animalFieldModel;

        public AnimalFieldModel FieldData { get => _animalFieldModel; }

        public void InitializeEmptyField(int length, int height)
        {
            if (length < 5 || height < 5)
            {
                throw new ArgumentException("Length or height can't be less than 5.");
            }

            _animalFieldModel = new AnimalFieldModel() 
            { 
                FieldLength = length,
                FieldHeight = height,
                Animals = new Animal?[height, length],
                Updates = 0
            };
        }

        public void Update()
        {
            if (_animalFieldModel == null)
            {
                throw new Exception("Animal field model can not be null.");
            }

            List<Animal> updatedAnimals = new();

            for (int row = 0; row < _animalFieldModel.FieldHeight; row++)
            {
                for (int col = 0; col < _animalFieldModel.FieldLength; col++)
                {
                    var animal = _animalFieldModel.Animals[row, col];

                    if(animal == null)
                    {
                        continue;
                    }

                    if(animal.Data.IsAlive == false && animal.GetType() != typeof(Ghost))
                    {
                        _animalFieldModel.Animals[row, col] = null;
                        continue;
                    }

                    var animalMove = GetAnimalMove(animal, row, col);

                    if (animalMove != null && !updatedAnimals.Contains(animal))
                    {
                        UpdateAnimal(animal, animalMove, row, col);
                    }

                    updatedAnimals.Add(animal);
                }
            }

            _animalFieldModel.Updates++;
        }

        private void UpdateAnimal(Animal? animal, Move? nextMove, int pos_row, int pos_col)
        {
            if(animal == null)
            {
                return;
            }

            if(nextMove != null)
            {
                var targetPosition = _animalFieldModel.Animals[pos_row + nextMove.RowChange, pos_col + nextMove.ColChange];
                if (targetPosition == null)
                {
                    _animalFieldModel.Animals[pos_row + nextMove.RowChange, pos_col + nextMove.ColChange] = animal;
                    _animalFieldModel.Animals[pos_row, pos_col] = null;
                }
                else if(IsOfType(animal, typeof(Lion)))
                {
                    if(IsOfType(targetPosition, typeof(Antelope)))
                    {
                        targetPosition.Die();
                        _animalFieldModel.Animals[pos_row + nextMove.RowChange, pos_col + nextMove.ColChange] = animal;
                        _animalFieldModel.Animals[pos_row, pos_col] = null;
                        animal.ChangeHealth(3);
                    }
                }

                
            }
        }

        private Move? GetAnimalMove(Animal? animal, int source_row, int source_col)
        {
            if(IsMovingAnimal(animal) == false)
            {
                return null;
            }

            int size = animal.Data.VisionRange * 2 + 1;
            AnimalFieldModel visionField = new () 
            {
                FieldLength = size,
                FieldHeight = size,
                Animals = new Animal?[size, size]
            };

            int center = visionField.FieldLength / 2;
            for (int row_offset = -animal.Data.VisionRange; row_offset <= animal.Data.VisionRange; row_offset++)
            {
                for (int col_offset = -animal.Data.VisionRange; col_offset <= animal.Data.VisionRange; col_offset++)
                {
                    if (InRange(source_row + row_offset, 0, _animalFieldModel.FieldHeight) && InRange(source_col + col_offset, 0, _animalFieldModel.FieldLength))
                    {
                        visionField.Animals[center + row_offset, center + col_offset] = _animalFieldModel.Animals[source_row + row_offset, source_col + col_offset];
                    }
                    else
                    {
                        visionField.Animals[center + row_offset, center + col_offset] = new Ghost();
                    }
                }
            }

            return animal.AttemptMove(visionField);
        }

        public Animal? AddAnimal(Animal newAnimal)
        {
            Tuple<int, int>? position = GetRandomAvailablePosition();

            if(position == null)
            {
                return null;
            }

            return AddAnimal(newAnimal, position.Item1, position.Item2);
        }

        public Animal? AddAnimal(Animal newAnimal, int row, int col)
        {
            if((row < 0 || row >= _animalFieldModel.FieldHeight) || (col < 0 || col >= _animalFieldModel.FieldLength))
            {
                throw new ArgumentException($"Row={row} or col={col} can't be less than field dimensions.");
            }

            if (_animalFieldModel.Animals[row, col] == null)
            {
                _animalFieldModel.Animals[row, col] = newAnimal;
                return newAnimal;
            }

            return null;
        }

        protected Tuple<int, int>? GetRandomAvailablePosition()
        {
            List<Tuple<int, int>> positions = new();

            for (int row = 0; row < _animalFieldModel.FieldHeight; row++)
            {
                for (int col = 0; col < _animalFieldModel.FieldLength; col++)
                {
                    if (_animalFieldModel.Animals[row, col] == null)
                    {
                        positions.Add(new Tuple<int, int>(row, col));
                    }
                }
            }

            if(positions.Count > 0)
            {
                Random random = new Random();
                return positions[random.Next(positions.Count)];
            }

            return null;
        }

        public static bool InRange(int value, int min, int max) => value >= min && value < max;

        public static bool IsMovingAnimal(Animal? animal) => animal != null && animal.Data.IsAlive && animal.GetType() != typeof(Ghost);

        public static bool IsOfType(object obj, Type type) => obj != null && obj.GetType() == type;
    }
}

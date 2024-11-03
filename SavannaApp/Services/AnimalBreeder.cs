using SavannaApp.Animals;
using SavannaApp.Interfaces;
using SavannaApp.Models;
using SavannaApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Services
{
    public class AnimalBreeder : IBreeder
    {
        private readonly IAnimalField _animalField;
        
        private AnimalFieldModel Field { get => _animalField.FieldData; }

        public int BreedingDistance { get; set; } = 1;
        public List<AnimalPair> Pairs { get; private set; }

        public AnimalBreeder(IAnimalField animalField)
        {
            _animalField = animalField;
        }

        public void Initialize(int breedingDistance)
        {
            if(breedingDistance < 1)
            {
                throw new ArgumentOutOfRangeException("Breeding distance can't be less than 1.");
            }
            
            BreedingDistance = breedingDistance;
            Pairs = new List<AnimalPair>();
        }

        public void Update()
        {
            int offset = BreedingDistance;

            BreedPairs();

            for (int row = 0; row < Field.FieldHeight; row++)
            {
                for (int col = 0; col < Field.FieldLength; col++)
                {
                    if (Field.Animals[row, col] != null)
                    {
                        TryBreedingAnimalAt(row, col);
                    }
                }
            }
        }

        public void TryBreedingAnimalAt(int row, int col)
        {
            var animal = Field.Animals[row, col];

            List<Animal> pairableAnimals = new();

            if (animal == null || animal.GetType() == typeof(Ghost))
            {
                return;
            }

            for(int row_offset = -BreedingDistance; row_offset <= BreedingDistance; row_offset++)
            {
                for(int col_offset = -BreedingDistance; col_offset <= BreedingDistance; col_offset++)
                {
                    if(row_offset == BreedingDistance && col_offset == BreedingDistance)
                    {
                        continue;
                    }

                    if(!InRange(row + row_offset, 0, Field.FieldHeight-1) || !InRange(col + col_offset, 0, Field.FieldLength-1))
                    {
                        continue;
                    }

                    var adjAnimal = Field.Animals[row + row_offset, col + col_offset];

                    if(adjAnimal == null)
                    {
                        continue;
                    }

                    var pair = AddAnimalPair(animal, adjAnimal);

                    if(pair != null)
                    {
                        pairableAnimals.Add(adjAnimal);
                    }
                }
            }

            RemoveNonPairablePairs(animal, pairableAnimals);
        }

        private AnimalPair? AddAnimalPair(Animal animal1, Animal animal2)
        {
            if(animal1.GetType() != animal2.GetType())
            {
                return null;
            }

            if(animal1 == animal2)
            {
                return null;
            }

            var existingPair = Pairs.Where(x => (x.Animal1.Equals(animal1) && x.Animal2.Equals(animal2)) || (x.Animal1.Equals(animal2) && x.Animal2.Equals(animal1))).ToList();
            if (existingPair.Any())
            {
                return existingPair.First();
            }

            var newPair = new AnimalPair()
            {
                Animal1 = animal1,
                Animal2 = animal2,
                PairFormedUpdate = Field.Updates
            };

            Pairs.Add(newPair);

            return newPair;
        }

        private void BreedPairs()
        {
            var breedablePairs = Pairs.Where(x => (Field.Updates - x.PairFormedUpdate == 3)).ToList();

            foreach(var pair in breedablePairs)
            {
                if(pair.Animal1.GetType() == typeof(Antelope))
                {
                    _animalField.AddAnimal(new Antelope());
                }
                else if (pair.Animal1.GetType() == typeof(Lion))
                {
                    _animalField.AddAnimal(new Lion());
                }

                Pairs.Remove(pair);
            }
        }

        private void RemoveNonPairablePairs(Animal animal, List<Animal> pairableAdjAnimals)
        {
            var oldPairsAnimalIsIn = Pairs.Where(x => (Field.Updates - x.PairFormedUpdate > 1) && (x.Animal1.Equals(animal) || x.Animal2.Equals(animal)))
                                        .ToList();

            foreach(var oldPair in oldPairsAnimalIsIn)
            {
                var adjAnimal = oldPair.Animal1.Equals(animal) ? oldPair.Animal2 : oldPair.Animal1;
                if (pairableAdjAnimals.Contains(adjAnimal) == false)
                {
                    Pairs.Remove(oldPair);
                }
            }
        }

        private bool InRange(int value, int min_inc, int max_inc) => value >= 0 && value <= max_inc;
    }
}

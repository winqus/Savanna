using Moq;
using SavannaApp.Animals;
using SavannaApp.Interfaces;
using SavannaApp.Models;
using SavannaApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Tests
{
    public class AnimalBreederTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Initialize_SetsVariables_Successfully(int input_distance)
        {
            // Arrange
            var mockAnimalField = new Mock<IAnimalField>();
            var breeder = new AnimalBreeder(mockAnimalField.Object);

            // Act
            breeder.Initialize(input_distance);

            // Assert
            Assert.Equal(input_distance, breeder.BreedingDistance);
            Assert.True(breeder.Pairs.Any() == false);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Initialize_Throws_ArgumentOutOfRange(int input_distance)
        {
            // Arrange
            var mockAnimalField = new Mock<IAnimalField>();
            var breeder = new AnimalBreeder(mockAnimalField.Object);

            // Act
            Exception ex = Record.Exception(() =>
                breeder.Initialize(input_distance)
            );

            // Assert
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        public void Update_Creates_SingleAnimalPair()
        {
            // Arrange
            var expectedPairCount = 1;
            var expectedPairFormedUpdate = 3;

            var fieldLength = 5;
            var fieldHeight = 6;
            var input_breedingDistance = 1;
            var animalFieldModel = new AnimalFieldModel()
            {
                Animals = new Animal?[fieldHeight, fieldLength],
                FieldHeight = fieldHeight,
                FieldLength = fieldLength,
                Updates = 3
            };
            animalFieldModel.Animals[0, 0] = new Antelope();
            animalFieldModel.Animals[1, 1] = new Antelope();
            var mockAnimalField = new Mock<IAnimalField>();
            mockAnimalField.SetupGet(x => x.FieldData).Returns(animalFieldModel);
            var breeder = new AnimalBreeder(mockAnimalField.Object);
            breeder.Initialize(input_breedingDistance);

            // Act
            breeder.Update();
            breeder.Update();
            breeder.Update();

            // Assert
            Assert.Equal(expectedPairCount, breeder.Pairs.Count());
            Assert.Equal(animalFieldModel.Animals[0, 0], breeder.Pairs.First().Animal2);
            Assert.Equal(expectedPairFormedUpdate, breeder.Pairs.First().PairFormedUpdate);
        }
    }
}

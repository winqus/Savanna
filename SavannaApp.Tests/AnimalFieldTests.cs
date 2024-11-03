using SavannaApp.Animals;
using SavannaApp.Models;
using SavannaApp.Services;

namespace SavannaApp.Tests
{
    public class AnimalFieldTests
    {
        [Theory]
        [InlineData(5, 5)]
        [InlineData(7, 6)]
        [InlineData(20, 5)]
        [InlineData(1000, 1000)]
        public void InitializeEmptyField_Assigns_AnimalFieldModel(int input_length, int input_height)
        {
            // Arrange
            var field = new AnimalField();

            // Act
            field.InitializeEmptyField(input_length, input_height);

            // Assert
            Assert.NotNull(field.FieldData);
            Assert.Equal(input_length, field.FieldData.FieldLength);
            Assert.Equal(input_height, field.FieldData.FieldHeight);
            Assert.Equal(input_length * input_height, field.FieldData.Animals.Length);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(3, 4)]
        [InlineData(4, 4)]
        [InlineData(4, 5)]
        [InlineData(6, 4)]
        public void InitializeEmptyField_Throws_ArgumentException(int input_length, int input_height)
        {
            // Arrange
            var field = new AnimalField();

            // Act
            Exception ex = Record.Exception(() =>
                field.InitializeEmptyField(input_length, input_height)
            );

            // Assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void Update_Throws_Exception()
        {
            // Arrange
            var field = new AnimalField();

            // Act
            Exception ex = Record.Exception(() =>
                field.Update()
            );

            // Assert
            Assert.IsType<Exception>(ex);
        }


        [Fact]
        public void Update_Executes_Correctly()
        {
            // Arrange
            var field = new AnimalField();
            field.InitializeEmptyField(5, 5);
            field.AddAnimal(new Lion(), 1, 1);
            field.AddAnimal(new Antelope(), 1, 2);

            // Act
            field.Update();

            // Assert
            Assert.Null(field.FieldData.Animals[1, 1]);
            Assert.IsType<Lion>(field.FieldData.Animals[1, 2]);
        }

        [Fact]
        public void AddAnimal_Returns_Animal()
        {
            // Arrange
            var field = new AnimalField();
            Animal returnedAnimal;

            // Act
            field.InitializeEmptyField(5, 5);
            returnedAnimal = field.AddAnimal(new Lion());

            // Assert
            Assert.NotNull(returnedAnimal);
        }

        [Fact]
        public void AddAnimal_Adds_4Animals()
        {
            // Arrange
            var field = new AnimalField();
            var expectedAnimalCount = 4;

            // Act
            field.InitializeEmptyField(5, 5);
            field.AddAnimal(new Lion());
            field.AddAnimal(new Lion());
            field.AddAnimal(new Antelope());
            field.AddAnimal(new Antelope());

            // Assert
            var count = 0;
            for (int row = 0; row < field.FieldData.FieldHeight; row++)
            {
                for (int col = 0; col < field.FieldData.FieldLength; col++)
                {
                    if (field.FieldData.Animals[row, col] != null)
                    {
                        count++;
                    }
                } 
            }
            Assert.Equal(expectedAnimalCount, count);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(3, 40)]
        [InlineData(30, 4)]
        [InlineData(30, 40)]
        public void AddAnimal_Throws_ArgumentException(int row, int col)
        {
            // Arrange
            var field = new AnimalField();
            field.InitializeEmptyField(10, 10);
            var input_animal = new Lion();

            // Act
            Exception ex = Record.Exception(() =>
                field.AddAnimal(input_animal, row, col)
            );

            // Assert
            Assert.IsType<ArgumentException>(ex);
        }
    }
}

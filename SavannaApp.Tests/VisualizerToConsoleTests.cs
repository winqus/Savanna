using Moq;
using SavannaApp.Animals;
using SavannaApp.Interfaces;
using SavannaApp.Models;
using SavannaApp.Services;
using SavannaApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavannaApp.Tests
{
    public class VisualizerToConsoleTests
    {
        [Fact]
        public void Visualize_Invokes_GameConsoleMethods()
        {
            // Arrange
            var fieldLength = 5;
            var fieldHeight = 6;
            var animalFieldModel = new AnimalFieldModel()
            {
                Animals = new Animal?[fieldHeight, fieldLength],
                FieldHeight = fieldHeight,
                FieldLength = fieldLength
            };
            animalFieldModel.Animals[0, 0] = new Lion();
            animalFieldModel.Animals[1, 3] = new Antelope();

            var mockConsole = new Mock<IGameConsole>();
            mockConsole.Setup(x => x.WriteLine(It.IsAny<string>()));
            mockConsole.Setup(x => x.Write(It.IsAny<string>()));
            var mockAnimalLoader = new Mock<IAnimalLoader>();
            mockAnimalLoader.Setup(x => x.GetAnimalTypes()).Returns(new List<Type>() { typeof(Antelope), typeof(Lion) });
            mockAnimalLoader.Setup(x => x.HasType(It.IsAny<Type>())).Returns(true);
            
            var visualizer = new VisualizerToConsole(mockConsole.Object, mockAnimalLoader.Object);

            // Act
            visualizer.Visualize(animalFieldModel);

            // Assert
            mockConsole.Verify(x => x.Clear(), Times.Once);
            mockConsole.Verify(x => x.WriteLine($"Field size: {animalFieldModel.FieldLength} x {animalFieldModel.FieldHeight}"), Times.Once);
            mockConsole.Verify(x => x.Write(It.IsIn("L", "A")), Times.Exactly(2));
            mockConsole.Verify(x => x.Write(visualizer.CharacterForEmpty), Times.Exactly(fieldLength * fieldHeight - 2));
            mockConsole.Verify(x => x.WriteLine(""), Times.Exactly(fieldHeight));
            mockConsole.Verify(x => x.WriteLine(It.IsRegex(@"^\[(L|A)\] \w+$")), Times.Exactly(2));
        }
    }
}

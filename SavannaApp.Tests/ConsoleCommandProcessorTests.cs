using Moq;
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
    public class ConsoleCommandProcessorTests
    {
        [Fact]
        public void Constructor_Assigns_InitialValues()
        {
            // Arrange
            var mockConsole = new Mock<IGameConsole>();

            // Act
            var commandProcessor = new ConsoleCommandProcessor(mockConsole.Object);
            var returnedCommand = commandProcessor.Command;

            // Assert
            Assert.Equal(Command.NONE, returnedCommand);
        }

        [Fact]

        public void Process_Invokes_IGameConsoleMethods()
        {
            // Arrange
            var keyPressCount = 3;
            var mockConsole = new Mock<IGameConsole>();
            mockConsole.SetupSequence(x => x.KeyAvailable)
                .Returns(true)
                .Returns(true)
                .Returns(true)
                .Returns(false);
            mockConsole.Setup(x => x.ReadKey(true)).Returns(It.IsAny<ConsoleKeyInfo>());
            var commandProcessor = new ConsoleCommandProcessor(mockConsole.Object);

            // Act
            commandProcessor.Process();
            commandProcessor.LogKey();

            // Assert
            mockConsole.Verify(x => x.ReadKey(true), Times.Exactly(keyPressCount));
            mockConsole.Verify(x => x.KeyAvailable, Times.Exactly(keyPressCount+1));
            Assert.Equal(Command.NONE, commandProcessor.Command);
            mockConsole.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Once);
            
        }
    }
}

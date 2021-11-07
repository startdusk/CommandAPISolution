using Xunit;

using CommandAPI.Models;
using System;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something awesome",
                Platform = "xUint",
                CommandLine = "dotnet test"
            };
        }
        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void TestCanChangeHowTo()
        {
            //Given
            var howTo = "Execute Unit Tests";

            //When
            testCommand.HowTo = howTo;

            //Then
            Assert.Equal(howTo, testCommand.HowTo);
        }

        [Fact]
        public void TestCanChangePlatform()
        {
            //Given
            var platform = ">NET<";

            //When
            testCommand.Platform = platform;

            //Then
            Assert.Equal(platform, testCommand.Platform);
        }
    }
}
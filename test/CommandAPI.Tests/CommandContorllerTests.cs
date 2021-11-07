using Xunit;
using System;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using Moq;

using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Models;
using System.Collections.Generic;
using CommandAPI.Profiles;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests
    {
        [Fact]
        public async void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Given
            var mockRepo = new Mock<ICommandAPIRepo>();
            mockRepo.Setup(repo => repo.GetAllCommands()).ReturnsAsync(this.GetCommands(0));

            var realProfile = new CommandsProfile();
            var configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile(realProfile));
            IMapper mapper = new Mapper(configuration);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //When
            var result = await controller.GetCommands();

            //Then
            Assert.IsType<OkObjectResult>(result.Result);
        }

        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command()
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".NET Core EF"
                });
            }

            return commands;
        }
    }
}
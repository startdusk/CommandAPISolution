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
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg =>
            cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            realProfile = null;
            configuration = null;
            mapper = null;
        }

        // 测试函数命名规范：测试的方法名_返回结果_条件
        // <method name>_<expected result>_<condition>

        [Fact]
        public async void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Given
            mockRepo.Setup(repo => repo.GetAllCommands()).ReturnsAsync(this.GetCommands(0));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //When
            var result = await controller.GetCommands();

            //Then
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            //Given
            mockRepo.Setup(repo => repo.GetAllCommands()).ReturnsAsync(this.GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //When
            var result = await controller.GetCommands();

            //Then
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;
            Assert.Single(commands);
        }

        [Fact]
        public async void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            //Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).ReturnsAsync(
                new Command
                {
                    Id = 1,
                    HowTo = "mock how to",
                    Platform = "mock platform",
                    CommandLine = "mock command line"
                }
            );
            var controller = new CommandsController(mockRepo.Object, mapper);

            //When
            var result = await controller.GetCommandById(1);

            //Then
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GetCommandByID_Returns200OK__WhenValidIDProvided()
        {
            //Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).ReturnsAsync(
                new Command
                {
                    Id = 1,
                    HowTo = "mock how to",
                    Platform = "mock platform",
                    CommandLine = "mock command line"
                }
            );
            var controller = new CommandsController(mockRepo.Object, mapper);

            //When
            var result = await controller.GetCommandById(1);

            //Then
            Assert.IsType<ActionResult<CommandReadDto>>(result);
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
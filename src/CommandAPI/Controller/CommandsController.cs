using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;

        public CommandsController(ICommandAPIRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(commandItems);
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null)
            {
                return NotFound($"can't found the command id: {id}");
            }
            return Ok(commandItem);
        }
    }
}
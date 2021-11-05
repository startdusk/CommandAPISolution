using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;
using CommandAPI.Helper;

namespace CommandAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommands()
        {
            var commandItems = await _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public async Task<ActionResult<CommandReadDto>> GetCommandById([FromRoute] int id)
        {
            var commandItem = await _repository.GetCommandById(id);
            if (commandItem == null)
            {
                return NotFound($"can't found the command id: {id}");
            }
            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDto>> CreateCommand([FromBody] CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            await _repository.CreateCommand(commandModel);
            await _repository.SaveChangesAsync();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCommand([FromRoute] int id, [FromBody] CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = await _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            await _repository.SaveChangesAsync();
            return NoContent();
        }


        // patch data
        // [
        //      { "op": "replace", "patch": "/how_to", "value": "Run a .NET Core 5 App" }
        // ]
        // 如何在ASP.NET Core中使用JSON Patch https://www.cnblogs.com/lwqlun/p/10433615.html
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialCommandUpdate([FromRoute] int id, [FromBody] JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = await _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCommand([FromRoute] int id)
        {
            var commandModelFromRepo = await _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFromRepo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        // 批量删除数据
        // url 示例 http://localhost:5000/api/commands/(1,2,3,4)
        [HttpDelete("({ids})")]
        public async Task<ActionResult> DeleteCommandByIds(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var commandModelsFromRepo = await _repository.GetCommandByIds(ids);
            _repository.DeleteCommands(commandModelsFromRepo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}

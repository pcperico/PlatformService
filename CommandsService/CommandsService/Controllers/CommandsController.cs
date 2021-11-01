using AutoMapper;
using CommandsService.Data.Repositories.Interfaces;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[Controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsforPlatform(int platformId)
        {
            Console.WriteLine($"--> Getting commands for platform id : {platformId}");
            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();
            var commands = _commandRepository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}",Name ="GetCommandForPlatform")]
        public ActionResult<CommandReadDto>GetCommandForPlatform(int platformId,int commandId)
        {
            Console.WriteLine($"--> Getting command id {commandId} for platform id : {platformId}");
            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();
            var command = _commandRepository.GetCommand(platformId, commandId);
            if (command == null)
                return NotFound();
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId,CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Creating command  for platform id : {platformId}");
            if (!_commandRepository.PlatformExists(platformId))
                return NotFound();
            var command = _mapper.Map<Command>(commandDto);
            _commandRepository.CreateCommand(platformId, command);
            return Ok(_mapper.Map<CommandReadDto>(command));

        }
    }
}

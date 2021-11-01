using AutoMapper;
using CommandsService.Data.Repositories.Interfaces;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
    [Route("api/c/[Controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepository commandRepository,IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlaform()
        {
            Console.WriteLine("--> Getting plat from command service");
            var plaforms = _commandRepository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(plaforms));
        }

        [HttpPost]
        public ActionResult Index()
        {
            Console.WriteLine("--> Inboud Post # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}

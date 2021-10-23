using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Repositories.Interfaces;
using PlatformService.Dtos;
using System.Collections.Generic;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepository platformRepository,IMapper mapper,ICommandDataClient commandDataClient)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>>GetPlatforms()
        {
            var platforms = _platformRepository.GetAllPlatforms();
            var model = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _platformRepository.GetPlatformById(id);
            if (platform == null)
                return NotFound();
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
           _platformRepository.CreatePlatform(platform);
            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
           try{
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
           }
           catch(Exception ex)
           {
                Console.WriteLine($"--> Could not sent sync: {ex.Message}");
           }
           return _mapper.Map<PlatformReadDto>(platform);
        }

        [HttpDelete]
        public ActionResult DeletePlatform(int id)
        {
            _platformRepository.DeletePlatform(id);
            return Ok();
        }


    }
}

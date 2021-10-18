using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Repositories.Interfaces;
using PlatformService.Dtos;
using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository platformRepository,IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
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
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
           _platformRepository.CreatePlatform(platform);
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

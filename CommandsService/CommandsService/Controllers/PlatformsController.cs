using Microsoft.AspNetCore.Mvc;
using System;

namespace CommandsService.Controllers
{
    [Route("api/c/[Controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {             

        }

        [HttpPost]
        public ActionResult Index()
        {
            Console.WriteLine("--> Inboud Post # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}

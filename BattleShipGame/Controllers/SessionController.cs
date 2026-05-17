using Microsoft.AspNetCore.Mvc;

namespace BattleShipGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSessions()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetSession(int id)
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult CreateSession()
        {
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteSession(int id)
        {
            return Ok();
        }
    }
}

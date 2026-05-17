using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BattleShipGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult CreateUser()
        {
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id)
        {
            return Ok();
        }
    }
}

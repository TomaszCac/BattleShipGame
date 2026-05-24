using BattleShipGame.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BattleShipGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepos;
        private readonly IUserRepository _userRepos;

        public SessionController(
            ISessionRepository sessionRepository,
            IUserRepository userRepository
        )
        {
            _sessionRepos = sessionRepository;
            _userRepos = userRepository;
        }

        [HttpGet]
        public IActionResult GetAllSessions()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetSession(int id)
        {
            return Ok(_sessionRepos.GetSession(id));
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

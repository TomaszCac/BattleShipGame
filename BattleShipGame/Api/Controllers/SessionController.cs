using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BattleShipGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepos;
        private readonly IUserRepository _userRepos;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public SessionController(
            ISessionRepository sessionRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IUserService userService
        )
        {
            _sessionRepos = sessionRepository;
            _userRepos = userRepository;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllSessions()
        {
            return Ok(
                JsonConvert.SerializeObject(
                    _mapper.Map<List<SessionDto>>(_sessionRepos.GetAvailableSessions())
                )
            );
        }

        [HttpGet("{id}")]
        public IActionResult GetSession(int id)
        {
            return Ok(_sessionRepos.GetSession(id));
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> CreateSession()
        {
            var user = await _userRepos.GetUserByIdAsync(_userService.GetId());
            return Ok(_sessionRepos.CreateSession(user));
        }

        [HttpGet("join/{id}"), Authorize]
        public async Task<IActionResult> JoinSession(int id)
        {
            var user = await _userRepos.GetUserByIdAsync(_userService.GetId());
            var session = _sessionRepos.JoinSession(user, id);
            return Ok(JsonConvert.SerializeObject(session));
        }

        [HttpDelete]
        public IActionResult DeleteSession(int id)
        {
            return Ok(_sessionRepos.RemoveSession(id));
        }
    }
}

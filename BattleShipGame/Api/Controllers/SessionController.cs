using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IGameNotificationService _sessionHub;

        public SessionController(
            ISessionRepository sessionRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IUserService userService,
            IGameNotificationService sessionHub)
        {
            _sessionRepos = sessionRepository;
            _userRepos = userRepository;
            _mapper = mapper;
            _userService = userService;
            _sessionHub = sessionHub;
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
        [HttpPost("place")]
        public async Task<IActionResult> PlaceBoard(
            [FromBody] string board,
            int sessionId,
            bool host
        )
        {
            _sessionRepos.SetBoard(JsonConvert.DeserializeObject<int[,]>(board), sessionId, host);

            if (_sessionRepos.CheckStart(sessionId))
            {
                await _sessionHub.StartFight(sessionId);
            }

            return Ok();
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

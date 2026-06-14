using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSessions()
        {
            return Ok(
                JsonConvert.SerializeObject(
                    _mapper.Map<List<SessionDto>>(_sessionRepos.GetAvailableSessions())
                )
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSession(int id)
        {
            return Ok(_sessionRepos.GetSession(id));
        }
        [HttpPost("place")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [HttpPost("shoot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ShootShip(int x, int y, int sessionId, bool turn)
        {
            if (_sessionRepos.ShootShip(x, y, sessionId, turn))
            {
                if (_sessionRepos.WinGame(sessionId, turn))
                {
                    await _sessionHub.WinGame(sessionId, turn);
                    var usersIds = _sessionRepos.GetUserIdsFromSession(sessionId);
                    await _userRepos.AddWinOrLose(usersIds.Item1, usersIds.Item2, turn);
                    _sessionRepos.EndSession(sessionId);
                    return Ok(true);
                }
                else
                {
                    await _sessionHub.ShipHit(x, y, sessionId, true);
                    return Ok(true);
                }
            }
            else
            {
                await _sessionHub.ShipHit(x, y, sessionId, false); 
                return Ok(false);
            }
        }
        [HttpDelete("end")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> EndGame(int sessionId, bool host)
        {
            var usersIds = _sessionRepos.GetUserIdsFromSession(sessionId);
            if (usersIds.Item2 != null)
            {
                await _userRepos.AddWinOrLose(usersIds.Item1, usersIds.Item2, !host);
            }
            _sessionRepos.EndSession(sessionId);
            return NoContent();
        }
        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSession()
        {
            var user = await _userRepos.GetUserByIdAsync(_userService.GetId());
            return Ok(_sessionRepos.CreateSession(user));
        }

        [HttpGet("join/{id}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinSession(int id)
        {
            var user = await _userRepos.GetUserByIdAsync(_userService.GetId());
            var session = _sessionRepos.JoinSession(user, id);
            return Ok(JsonConvert.SerializeObject(session));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteSession(int id)
        {
            return Ok(_sessionRepos.RemoveSession(id));
        }
    }
}

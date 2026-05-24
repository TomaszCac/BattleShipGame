using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BattleShipGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepos;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserRepository userRepos, IUserService userService)
        {
            _mapper = mapper;
            _userRepos = userRepos;
            _userService = userService;
        }

        [HttpGet("name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name is empty");
            var user = await _userRepos.GetUserByUserNameAsync(name);
            if (user != null)
                return Ok(_mapper.Map<UserDto>(user));
            return NotFound("User Name not found");
        }

        [HttpGet("id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("ID is empty");
            var user = await _userRepos.GetUserByIdAsync(id);
            if (user != null)
                return Ok(_mapper.Map<UserDto>(user));
            return NotFound("User ID not found");
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
            {
                var result = await _userRepos.CreateUserAsync(_mapper.Map<User>(user), user.Password);
                if (result.Success)
                {
                    return Ok("User registered successfully");
                }
                if(result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    return Conflict("Username already taken");
                }
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            return BadRequest("User data is not complete");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            return Ok(
                _userService.Login(
                    await _userRepos.GetUserByUserNameAsync(userDto.UserName),
                    userDto.Password
                )
            );
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return Ok(await _userRepos.DeleteUserAsync(id));
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user, string id)
        {
            var modifiedUser = _mapper.Map<User>(user);
            modifiedUser.Id = id;
            return Ok(await _userRepos.UpdateUserAsync(modifiedUser));
        }
    }
}

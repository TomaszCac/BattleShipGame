using AutoMapper;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Application.Interfaces;
using BattleShipGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                return BadRequest("User Name is empty");
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
                return BadRequest("User ID is empty");
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
                var result = await _userRepos.CreateUserAsync(
                    _mapper.Map<User>(user),
                    user.Password
                );
                if (result.Success)
                {
                    return Ok("User registered successfully");
                }
                if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    return Conflict("User Name already taken");
                }
                return BadRequest(JsonConvert.SerializeObject(result.Errors));
            }
            return BadRequest("User data is not complete");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            if (!string.IsNullOrEmpty(userDto.UserName) && !string.IsNullOrEmpty(userDto.Password))
            {
                var userResult = await _userRepos.GetUserByUserNameAsync(userDto.UserName);

                if (userResult != null)
                {
                    var loginResult = _userService.Login(userResult, userDto.Password);
                    if (loginResult != null)
                        return Ok(loginResult);
                }

                return BadRequest("Wrong password or username");
            }
            return BadRequest("User data is not complete");
        }
        [HttpGet("current"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(
                _mapper.Map<UserDto>(await _userRepos.GetUserByIdAsync(_userService.GetId()))
            );
        }


        [HttpDelete, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser()
        {
            return Ok(await _userRepos.DeleteUserAsync(_userService.GetId()));
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                var modifiedUser = _mapper.Map<User>(user);
                modifiedUser.Id = _userService.GetId();
                var result = await _userRepos.UpdateUserAsync(modifiedUser);
                if (result.Success)
                    return Ok("User updated successfully");
                return BadRequest(JsonConvert.SerializeObject(result.Errors));
            }
            return BadRequest("User data is not complete");
        }
    }
}

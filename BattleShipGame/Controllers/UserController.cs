using AutoMapper;
using BattleShipGame.Dtos;
using BattleShipGame.Interfaces;
using BattleShipGame.Models;
using BattleShipGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BattleShipGame.Controllers
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
        public async Task<IActionResult> GetUserByName(string name)
        {
            return Ok(_mapper.Map<UserDto>(await _userRepos.GetUserByUserNameAsync(name)));
        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(_mapper.Map<UserDto>(await _userRepos.GetUserByIdAsync(id)));
        }
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            return Ok(await _userRepos.CreateUserAsync(_mapper.Map<User>(user), user.Password));

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            return Ok(_userService.Login(await _userRepos.GetUserByUserNameAsync(userDto.UserName),userDto.Password));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return Ok(await _userRepos.DeleteUserAsync(id));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody]UserDto user, string id)
        {
            var modifiedUser = _mapper.Map<User>(user);
            modifiedUser.Id = id;
            return Ok(await _userRepos.UpdateUserAsync(modifiedUser));
        }
    }
}

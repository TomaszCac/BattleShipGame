using AutoMapper;
using BattleShipGame.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BattleShipGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }
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
